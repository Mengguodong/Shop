using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace SFO2O.Framework.Uitl
{
    public static class ZipHelper
	{
        /// <summary>
        /// 表示一个返回的文件
        /// </summary>
        public struct UnzipppedFileEntry
        {
            public string orig_name;
            public string new_name;
            public string full_path;

            public UnzipppedFileEntry(string orig_name, string new_name, string full_path)
            {
                this.orig_name = orig_name;
                this.new_name = new_name;
                this.full_path = full_path;
            }
        }

        /// <summary>
        /// 解压缩一个zip文件，不支持密码，不支持多级目录
        /// 所有解压缩出来的文件名都被替换成临时文件名，但保留后缀名，放到系统的临时目录中
        /// </summary>
        /// <param name="zipPathAndFile">输入的zip文件名</param>
        /// <param name="outputFolder">输出的目录</param>
        /// <param name="deleteZipFile">解压成功后是否删除原文件</param>
        /// <returns>返回解压缩结果的目录名列表</returns>
        public static ArrayList UnZipFiles(string zipPathAndFile, string outputFolder, bool deleteZipFile)
        {
            var file_name_list = new ArrayList();
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = Path.GetDirectoryName(zipPathAndFile);

            if (outputFolder.Substring(outputFolder.Length - 1, 1) != "\\")
                outputFolder = outputFolder + "\\";

            var s = new ZipInputStream(File.OpenRead(zipPathAndFile));
            ZipEntry theEntry;

            while ((theEntry = s.GetNextEntry()) != null)
            {
                string fileName = Path.GetFileName(theEntry.Name);
                if (Path.GetExtension(fileName) == string.Empty)
                    continue;

                string newName = StringHelper.GetRandomString(10) + Path.GetExtension(fileName);
                string fullPath = outputFolder + newName;

                var ufe = new UnzipppedFileEntry(fileName, newName, fullPath);
                file_name_list.Add(ufe);

                FileStream streamWriter = File.Create(fullPath);

                int size = 2048;

                byte[] data = new byte[2048];

                while (true)
                {
                    size = s.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        streamWriter.Write(data, 0, size);
                    }
                    else
                    {
                        break;

                    }
                }
                streamWriter.Close();
            }

            s.Close();

            if (deleteZipFile)
                File.Delete(zipPathAndFile);

            return file_name_list;
        }//function
	}
}