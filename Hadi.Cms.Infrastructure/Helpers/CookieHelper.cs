using System;
using System.Web;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public class CookieHelper
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            HttpCookie encodedCookie = HttpCookieEncryption.Encrypt(new HttpCookie(key, value));

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }

        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                // For security purpose, we need to encrypt the value.
                HttpCookie decodedCookie = HttpCookieEncryption.Decrypt(cookie);
                value = decodedCookie.Value;
            }
            return value;
        }

        public static bool IsExist(string key)
        {
            bool result = HttpContext.Current.Request.Cookies[key] != null;

            return result;
        }

        public static void RemoveCookie(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var currentCookie = new HttpCookie(key);
                currentCookie.Expires = DateTime.Now.AddDays(-1);

                HttpContext.Current.Request.Cookies.Add(currentCookie);
            }
        }
    }
}
