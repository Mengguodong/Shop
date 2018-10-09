using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public class ReflectionHelper
    {
        public static T ReflectionCopy<T>(T source, T target) where T : new()
        {
            var t = new T();
            Type type = t.GetType();

            foreach (var property in type.GetProperties())
            {
                var sourcev = property.GetValue(source, null);
                property.SetValue(target, sourcev);
            }
            return target;
        }

        public static Dictionary<string, string> ReflectionKeyValue<T>(T source)
        {
            Type type = source.GetType();

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var property in type.GetProperties())
            {
                var sourcev = property.GetValue(source, null);
                if (property.PropertyType == typeof(DateTime))
                {
                    if (sourcev != null)
                    {
                        result.Add(property.Name, ((DateTime)sourcev).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        result.Add(property.Name, "");
                    }

                }
                else
                {
                    result.Add(property.Name, sourcev == null ? "" : sourcev.ToString());
                }
            }
            return result;
        }
    }
}
