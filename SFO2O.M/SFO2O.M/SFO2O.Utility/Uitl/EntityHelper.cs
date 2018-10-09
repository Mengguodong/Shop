using System;
using System.Web;

namespace SFO2O.Utility.Uitl
{
    public class EntityHelper
    {
        public static int GetInt(string key, int defaultValue, HttpContext context)
        {
            int temp = defaultValue;

            try
            {
                if (context.Request[key] != null)
                {
                    int.TryParse(context.Request[key].ToString(), out temp);
                }

                return temp;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetString(string key, HttpContext context)
        {
            string temp = "";

            try
            {
                if (context.Request[key] != null)
                {
                    return context.Request[key].ToString();
                }

                return temp;
            }
            catch
            {
                return "";
            }
        }

        public static T GetEntity<T>(T t, HttpContext context)
        {
            foreach (var key in context.Request.Form.AllKeys)
            {
                if (!String.IsNullOrEmpty(context.Request.Form[key]))
                {
                    var prop = t.GetType().GetProperty(key);
                    if (prop != null)
                    {
                        if (prop.PropertyType == typeof(Int64))
                            prop.SetValue(t, Convert.ToInt64(context.Request.Form[key]), null);
                        else if (prop.PropertyType == typeof(Int32))
                            prop.SetValue(t, Convert.ToInt32(context.Request.Form[key]), null);
                        else if (prop.PropertyType == typeof(Decimal))
                            prop.SetValue(t, Convert.ToDecimal(context.Request.Form[key]), null);
                        else if (prop.PropertyType == typeof(DateTime?))
                            try
                            {
                                prop.SetValue(t, Convert.ToDateTime(context.Request.Form[key]), null);
                            }
                            catch
                            {

                            }
                        else if (prop.PropertyType == typeof(Boolean))
                            try
                            {
                                prop.SetValue(t, Convert.ToBoolean(context.Request.Form[key]), null);
                            }
                            catch
                            {

                            }
                        else if (prop.PropertyType == typeof(DateTime))
                            try
                            {
                                prop.SetValue(t, Convert.ToDateTime(context.Request.Form[key]), null);
                            }
                            catch { }
                        else if (prop.PropertyType == typeof(double))
                            try
                            {
                                prop.SetValue(t, ConvertHelper.ZParseDouble(context.Request.Form[key], 1.0), null);
                            }
                            catch { }
                        else
                            prop.SetValue(t, context.Request.Form[key], null);
                    }
                }
            }
            return t;
        }
    }
}
