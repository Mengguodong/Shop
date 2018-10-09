using System.Web;

namespace SFO2O.Utility.Extensions
{
    public static class HttpRequestExtension
    {
        public static bool IsSsl(this HttpRequest request)
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }
        
        public static bool IsSsl(this HttpRequestBase request)
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }

        public static string GetScheme(this HttpRequest request)
        {
            return request.IsSsl() ? "https" : "http";
        }

        public static string GetScheme(this HttpRequestBase request)
        {
            return request.IsSsl() ? "https" : "http";
        }
    }
}
