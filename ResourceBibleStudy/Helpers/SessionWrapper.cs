using System.Web;

namespace ResourceBibleStudy.Helpers
{
    public class SessionWrapper
    {
        public static T GetFromSession<T>(string key)
        {
            var obj = HttpContext.Current.Session[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        public static void SetInSession<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(key);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static T GetFromApplication<T>(string key)
        {
            return (T)HttpContext.Current.Application[key];
        }

        public static void SetInApplication<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Application.Remove(key);
            }
            else
            {
                HttpContext.Current.Application[key] = value;
            }
        }
    }
}