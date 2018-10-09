using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SFO2O.EntLib.DataExtensions.Basic
{
    /// <summary>
    /// 参数检查
    /// </summary>
    internal static class Check
    {
        public static void Valid(bool @checked, string msg)
        {
            if (@checked)
                throw new ArgumentException(msg);
        }
        public static void Valid(bool @checked, string format, params object[] args)
        {
            if (@checked)
                throw new ArgumentException(string.Format(format, args));
        }
        public static void IsNull(object obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }
        public static void IsNull(object obj, string paramName, string msg)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, msg);
        }
        public static void IsNull(object obj, string paramName, string format, params object[] args)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, string.Format(format, args));
        }
        public static void IsNullOrEmpty(object obj, string msg)
        {
            if (obj == null)
                throw new ArgumentException(msg);
            if (obj is string)
                if (string.IsNullOrEmpty(obj as string))
                    throw new ArgumentException(msg);
        }
        public static void IsNullOrEmpty(ICollection obj, string msg)
        {
            if (obj == null || obj.Count == 0)
                throw new ArgumentException(msg);
        }
    }
}
