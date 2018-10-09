using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SFO2O.M.Controllers.Binder
{
    public class JsonBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string jsonText = controllerContext.HttpContext.Request[bindingContext.ModelName];

            if (jsonText != null)
            {
                return new JavaScriptSerializer().Deserialize<dynamic>(jsonText);
            }

            return null;
        }
    }

    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            StreamReader reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string json = reader.ReadToEnd();

            if (string.IsNullOrEmpty(json))
            {
                return json;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object jsonData = serializer.DeserializeObject(json);
            return serializer.Deserialize<T>(json);
        }
    }
}
