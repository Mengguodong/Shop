using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Collections;

namespace SFO2O.Supplier.Common
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Helper
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Int32[] GetIndexArray(Type type, DataTable dt)
        {
            var Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var arrIndex = new Int32[Properties.Length];
            for (int i = 0; i < arrIndex.Length; i++)
            {
                arrIndex[i] = -1;
            }
            var Columns = dt.Columns;
            for (int i = 0; i < Columns.Count; i++)
            {
                var pIndex = Columns[i].GetIndex(Properties);
                if (pIndex >= 0)
                {
                    arrIndex[pIndex] = i;
                }
            }
            return arrIndex;
        }
        internal static Int32 GetIndex(this DataColumn Column, PropertyInfo[] Properties)
        {
            var dataType = Column.DataType;
            for (Int32 i = 0; i < Properties.Length; i++)
            {
                var PropertyInfo = Properties[i];
                if (!PropertyInfo.CanWrite)
                {
                    break;
                }
                if (String.Compare(Properties[i].Name, Column.ColumnName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var type = PropertyInfo.PropertyType;
                    if (!type.IsEquivalentTo(dataType))
                    {
                        if (type.IsValueType)
                        {
                            if (type.IsGenericType && !type.IsGenericTypeDefinition && EmitStatic.NullableType == type.GetGenericTypeDefinition())
                            {
                                type = type.GetGenericArguments()[0];
                            }
                        }
                        if (type.IsEnum)
                        {
                            var enumBaseType = type.GetEnumUnderlyingType();
                            if (!dataType.IsEquivalentTo(enumBaseType))
                            {
                                throw new ArgumentException(string.Format("枚举类型“{0}”的基础类型“{1}”与数据类型“{2}”不匹配。", type, enumBaseType, dataType));
                            }
                        }
                    }
                    return i;
                }
            }
            return -1;
        }
        internal static void Emit_OpCodes_Ldc_I4(this ILGenerator il, Int32 value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (value > 0 && value < 128)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }
        /// <summary>
        /// 遍历属性生成赋值IL指令
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arrIndex"></param>
        /// <param name="index"></param>
        /// <param name="entity"></param>
        /// <param name="loadDataRow"></param>
        internal static void Emit_GenerateSetPropertiesIL(this ILGenerator il, Type type,
            LocalBuilder arrIndex, LocalBuilder index, LocalBuilder entity, Action loadDataRow)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                var pi = properties[i];
                if (!pi.CanWrite)
                {
                    continue;
                }
                var propertyType = pi.PropertyType;
                var toType = propertyType;
                bool isNullable = false;
                if (propertyType.IsValueType)
                {
                    if (propertyType.IsGenericType && !propertyType.IsGenericTypeDefinition && EmitStatic.NullableType == propertyType.GetGenericTypeDefinition())
                    {
                        isNullable = true;
                        var InnerType = propertyType.GetGenericArguments()[0];
                        if (InnerType.IsEnum)
                        {
                            toType = InnerType;
                        }
                    }
                }
                else
                {
                    isNullable = true;
                }
                var label1 = il.DefineLabel();
                il.Emit(OpCodes.Ldloc, arrIndex);
                il.Emit_OpCodes_Ldc_I4(i);
                il.Emit(OpCodes.Ldelem_I4);
                il.Emit(OpCodes.Stloc, index);
                il.Emit(OpCodes.Ldloc, index);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Blt_S, label1);
                loadDataRow();
                il.Emit(OpCodes.Ldloc, index);
                il.Emit(OpCodes.Callvirt, EmitStatic.IsNull);
                if (!isNullable)
                {
                    var label2 = il.DefineLabel();
                    il.Emit(OpCodes.Brtrue_S, label2);
                    il.Emit(OpCodes.Ldloc, entity);
                    loadDataRow();
                    il.Emit(OpCodes.Ldloc, index);
                    il.Emit(OpCodes.Callvirt, EmitStatic.GetItemByIndex);
                    il.Emit(OpCodes.Unbox_Any, toType);
                    il.Emit(OpCodes.Callvirt, pi.GetSetMethod());
                    il.Emit(OpCodes.Br_S, label1);
                    il.MarkLabel(label2);
                    il.Emit(OpCodes.Ldstr, String.Format("类型“{0}”的属性“{1}”不允许Null值", type.Name, pi.Name));
                    il.Emit(OpCodes.Newobj, EmitStatic.NullReferenceExceptionCtor);
                    il.Emit(OpCodes.Throw);
                }
                else
                {
                    il.Emit(OpCodes.Brtrue_S, label1);
                    il.Emit(OpCodes.Ldloc, entity);
                    loadDataRow();
                    il.Emit(OpCodes.Ldloc, index);
                    il.Emit(OpCodes.Callvirt, EmitStatic.GetItemByIndex);
                    if (propertyType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, toType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Castclass, propertyType);
                    }
                    il.Emit(OpCodes.Callvirt, pi.GetSetMethod());
                }
                il.MarkLabel(label1);
            }
        }

        internal static Converter<TData, T> CreateConverter<TData, T>(this DynamicMethod dm)
        {
            return (Converter<TData, T>)dm.CreateDelegate(typeof(Converter<TData, T>));
        }
    }

    internal static class EmitStatic
    {
        internal static readonly Type NullableType = typeof(Nullable<>);
        internal static readonly Type Int32Type = typeof(Int32);
        internal static readonly Type Int32ArrayType = typeof(Int32[]);
        internal static readonly Type DataTableType = typeof(DataTable);
        internal static readonly Type DataRowType = typeof(DataRow);
        internal static readonly Type DataRowCollectionType = typeof(DataRowCollection);
        internal static readonly Type IEnumeratorType = typeof(IEnumerator);
        internal static readonly Type IDisposableType = typeof(IDisposable);

        internal static readonly MethodInfo GetRows = DataTableType.GetMethod("get_Rows");
        internal static readonly MethodInfo GetTable = DataRowType.GetMethod("get_Table");
        internal static readonly MethodInfo GetIndexArray = typeof(Helper).GetMethod("GetIndexArray");
        internal static readonly MethodInfo GetItemByIndex = DataRowType.GetMethod("get_Item", new[] { Int32Type });
        internal static readonly MethodInfo IsNull = DataRowType.GetMethod("IsNull", new[] { Int32Type });
        internal static readonly MethodInfo GetRowsCount = DataRowCollectionType.GetMethod("get_Count");
        internal static readonly MethodInfo GetRowsEnumerator = DataRowCollectionType.GetMethod("GetEnumerator");
        internal static readonly MethodInfo GetCurrent = IEnumeratorType.GetMethod("get_Current");
        internal static readonly MethodInfo MoveNext = IEnumeratorType.GetMethod("MoveNext");
        internal static readonly MethodInfo Dispose = IDisposableType.GetMethod("Dispose");
        internal static readonly MethodInfo GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle", new[] { typeof(RuntimeTypeHandle) });

        internal static readonly ConstructorInfo NullReferenceExceptionCtor = typeof(NullReferenceException).GetConstructor(new[] { typeof(String) });
    }

    /// <summary>
    /// DataTable相关扩展方法
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// 用Emit动态生成的方法把DataRow转换为实体类T
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataRow dr) where T : class,new()
        {
            return DataRowConvert<T>.Convert(dr);
        }

        /// <summary>
        /// 用Emit动态生成的方法把DataTable转换为实体类T的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToEntityList<T>(this DataTable dt) where T : class,new()
        {
            return DataTableConvert<T>.Convert(dt);
        }
    }

    internal static class DataRowConvert<T> where T : class,new()
    {
        internal static readonly Converter<DataRow, T> Convert = Create();
        private static Converter<DataRow, T> Create()
        {
            var type = typeof(T);
            var method = new DynamicMethod("ConvertDataRowTo" + type.Name, type, new[] { EmitStatic.DataRowType }, type);
            var il = method.GetILGenerator();
            var arrIndex = il.DeclareLocal(EmitStatic.Int32ArrayType);
            var entity = il.DeclareLocal(type);
            var index = il.DeclareLocal(EmitStatic.Int32Type);
            var label1 = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Brtrue_S, label1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            il.MarkLabel(label1);
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, EmitStatic.GetTypeFromHandle);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetTable);
            il.Emit(OpCodes.Call, EmitStatic.GetIndexArray);
            il.Emit(OpCodes.Stloc, arrIndex);
            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, entity);
            il.Emit_GenerateSetPropertiesIL(type, arrIndex, index, entity, () => { il.Emit(OpCodes.Ldarg_0); });
            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Ret);
            return method.CreateConverter<DataRow, T>();
        }
    }

    internal static class DataTableConvert<T> where T : class,new()
    {
        internal static readonly Converter<DataTable, List<T>> Convert = Create();
        private static Converter<DataTable, List<T>> Create()
        {
            var type = typeof(T);
            var listType = typeof(List<T>);
            var method = new DynamicMethod("ConvertDataTableToList" + type.Name, listType, new[] { EmitStatic.DataTableType }, type);
            var il = method.GetILGenerator();
            var arrIndex = il.DeclareLocal(EmitStatic.Int32ArrayType);
            var list = il.DeclareLocal(listType);
            var index = il.DeclareLocal(EmitStatic.Int32Type);
            var dr = il.DeclareLocal(EmitStatic.DataRowType);
            var entity = il.DeclareLocal(type);
            var enumerator = il.DeclareLocal(EmitStatic.IEnumeratorType);
            var disposable = il.DeclareLocal(EmitStatic.IDisposableType);
            var label1 = il.DefineLabel();
            var label2 = il.DefineLabel();
            var label3 = il.DefineLabel();
            var label4 = il.DefineLabel();
            var label5 = il.DefineLabel();
            var label6 = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Brtrue_S, label1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            il.MarkLabel(label1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetRows);
            il.Emit(OpCodes.Callvirt,EmitStatic.GetRowsCount);
            il.Emit(OpCodes.Brtrue_S, label2);
            il.Emit(OpCodes.Newobj, listType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);
            il.MarkLabel(label2);
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, EmitStatic.GetTypeFromHandle);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, EmitStatic.GetIndexArray);
            il.Emit(OpCodes.Stloc, arrIndex);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetRows);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetRowsCount);
            il.Emit(OpCodes.Newobj, listType.GetConstructor(new[] { EmitStatic.Int32Type }));
            il.Emit(OpCodes.Stloc, list);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetRows);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetRowsEnumerator);
            il.Emit(OpCodes.Stloc, enumerator);
            il.BeginExceptionBlock();
            il.Emit(OpCodes.Br, label4);
            il.MarkLabel(label3);
            il.Emit(OpCodes.Ldloc, enumerator);
            il.Emit(OpCodes.Callvirt, EmitStatic.GetCurrent);
            il.Emit(OpCodes.Castclass, EmitStatic.DataRowType);
            il.Emit(OpCodes.Stloc, dr);
            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, entity);
            il.Emit_GenerateSetPropertiesIL(type, arrIndex, index, entity, () => { il.Emit(OpCodes.Ldloc, dr); });
            il.Emit(OpCodes.Ldloc, list);
            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Callvirt, listType.GetMethod("Add"));
            il.MarkLabel(label4);
            il.Emit(OpCodes.Ldloc, enumerator);
            il.Emit(OpCodes.Callvirt, EmitStatic.MoveNext);
            il.Emit(OpCodes.Brtrue, label3);
            il.Emit(OpCodes.Leave_S, label6);
            il.BeginFinallyBlock();
            il.Emit(OpCodes.Ldloc, enumerator);
            il.Emit(OpCodes.Isinst, EmitStatic.IEnumeratorType);
            il.Emit(OpCodes.Stloc, disposable);
            il.Emit(OpCodes.Ldloc, disposable);
            il.Emit(OpCodes.Brfalse_S, label5);
            il.Emit(OpCodes.Ldloc, disposable);
            il.Emit(OpCodes.Callvirt, EmitStatic.Dispose);
            il.MarkLabel(label5);
            il.EndExceptionBlock();
            il.MarkLabel(label6);
            il.Emit(OpCodes.Ldloc, list);
            il.Emit(OpCodes.Ret);
            return method.CreateConverter<DataTable, List<T>>();
        }
    }
}
