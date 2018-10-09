using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    internal enum SerializedType : byte
    {
        ByteArray = 0,
        Object = 1,
        String = 2,
        Datetime = 3,
        Bool = 4,
        //SByte		= 5, //Makes no sense.
        Byte = 6,
        Short = 7,
        UShort = 8,
        Int = 9,
        UInt = 10,
        Long = 11,
        ULong = 12,
        Float = 13,
        Double = 14,

        CompressedByteArray = 250, ///为了保证只有一个byte，就用了255以下的数
        CompressedObject = 251,
        CompressedString = 252,
    }

    public static class Serializer
    {
        private const uint compressionThreshold = 16 * 1024; //128K以上压缩
        private static BinaryFormatter SHARED_BF = InitBinaryFOrmatter();

        /// <summary>
        /// 初始化一个序列化对象，设置一些参数
        /// </summary>
        /// <returns></returns>
        private static BinaryFormatter InitBinaryFOrmatter()
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            return bf;
        }

        public static byte[] Serialize(object value)
        {
            SerializedType type;
            byte[] bytes;
            if (value is byte[])
            {
                bytes = (byte[])value;
                type = SerializedType.ByteArray;
                if (bytes.Length > compressionThreshold)
                {
                    bytes = compress(bytes);
                    type = SerializedType.CompressedByteArray;
                }
            }
            else if (value is string)
            {
                bytes = Encoding.UTF8.GetBytes((string)value);
                type = SerializedType.String;
                if (bytes.Length > compressionThreshold)
                {
                    bytes = compress(bytes);
                    type = SerializedType.CompressedString;
                }
            }
            else if (value is DateTime)
            {
                bytes = BitConverter.GetBytes(((DateTime)value).Ticks);
                type = SerializedType.Datetime;
            }
            else if (value is bool)
            {
                bytes = new byte[] { (byte)((bool)value ? 1 : 0) };
                type = SerializedType.Bool;
            }
            else if (value is byte)
            {
                bytes = new byte[] { (byte)value };
                type = SerializedType.Byte;
            }
            else if (value is short)
            {
                bytes = BitConverter.GetBytes((short)value);
                type = SerializedType.Short;
            }
            else if (value is ushort)
            {
                bytes = BitConverter.GetBytes((ushort)value);
                type = SerializedType.UShort;
            }
            else if (value is int)
            {
                bytes = BitConverter.GetBytes((int)value);
                type = SerializedType.Int;
            }
            else if (value is uint)
            {
                bytes = BitConverter.GetBytes((uint)value);
                type = SerializedType.UInt;
            }
            else if (value is long)
            {
                bytes = BitConverter.GetBytes((long)value);
                type = SerializedType.Long;
            }
            else if (value is ulong)
            {
                bytes = BitConverter.GetBytes((ulong)value);
                type = SerializedType.ULong;
            }
            else if (value is float)
            {
                bytes = BitConverter.GetBytes((float)value);
                type = SerializedType.Float;
            }
            else if (value is double)
            {
                bytes = BitConverter.GetBytes((double)value);
                type = SerializedType.Double;
            }
            else
            {
                //Object
                using (MemoryStream ms = new MemoryStream())
                {
                    SHARED_BF.Serialize(ms, value);
                    bytes = ms.ToArray();
                    type = SerializedType.Object;
                    if (bytes.Length > compressionThreshold)
                    {
                        bytes = compress(bytes);
                        type = SerializedType.CompressedObject;
                    }
                }
            }

            byte[] result_bytes = new byte[bytes.Length + 1];
            result_bytes[0] = (byte)type;

            System.Buffer.BlockCopy(bytes, 0, result_bytes, 1, bytes.Length);

            return result_bytes;
        }

        private static byte[] compress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream gzs = new DeflateStream(ms, CompressionMode.Compress, false))
                {
                    gzs.Write(bytes, 0, bytes.Length);
                }
                ms.Close();
                return ms.ToArray();
            }
        }

        //跳过第一个byte，把其余的给Decompress了
        private static byte[] decompress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes, 1, bytes.Length - 1, false))
            {
                using (DeflateStream gzs = new DeflateStream(ms, CompressionMode.Decompress, false))
                {
                    using (MemoryStream dest = new MemoryStream())
                    {
                        dest.Write(bytes, 0, 1); //把头写回去

                        byte[] tmp = new byte[bytes.Length];
                        int read;

                        while ((read = gzs.Read(tmp, 1, tmp.Length - 1)) != 0)
                        {
                            dest.Write(tmp, 1, read);
                        }
                        dest.Close();
                        return dest.ToArray();
                    }
                }
            }
        }

        public static object DeSerialize(byte[] bytes)
        {
            SerializedType type = (SerializedType)bytes[0];

            switch (type)
            {
                case SerializedType.String:
                    return Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
                case SerializedType.Datetime:
                    return new DateTime(BitConverter.ToInt64(bytes, 1));
                case SerializedType.Bool:
                    return bytes[1] == 1;
                case SerializedType.Byte:
                    return bytes[1];
                case SerializedType.Short:
                    return BitConverter.ToInt16(bytes, 1);
                case SerializedType.UShort:
                    return BitConverter.ToUInt16(bytes, 1);
                case SerializedType.Int:
                    return BitConverter.ToInt32(bytes, 1);
                case SerializedType.UInt:
                    return BitConverter.ToUInt32(bytes, 1);
                case SerializedType.Long:
                    return BitConverter.ToInt64(bytes, 1);
                case SerializedType.ULong:
                    return BitConverter.ToUInt64(bytes, 1);
                case SerializedType.Float:
                    return BitConverter.ToSingle(bytes, 1);
                case SerializedType.Double:
                    return BitConverter.ToDouble(bytes, 1);
                case SerializedType.Object:
                    using (MemoryStream ms = new MemoryStream(bytes, 1, bytes.Length - 1))
                    {
                        return SHARED_BF.Deserialize(ms);
                    }
                case SerializedType.CompressedByteArray:
                    {
                        bytes[0] = (byte)SerializedType.ByteArray;
                        return DeSerialize(decompress(bytes));
                    }
                case SerializedType.CompressedString:
                    {
                        bytes[0] = (byte)SerializedType.String;
                        return DeSerialize(decompress(bytes));
                    }
                case SerializedType.CompressedObject:
                    {
                        bytes[0] = (byte)SerializedType.Object;
                        return DeSerialize(decompress(bytes));
                    }
                case SerializedType.ByteArray:
                default:
                    return bytes;
            }
        }
    }
}
