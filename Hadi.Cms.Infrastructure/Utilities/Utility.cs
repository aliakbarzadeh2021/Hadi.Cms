using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hadi.Cms.Infrastructure.Utilities
{
    public static class Utility
    {
        public static CultureInfo GetCultureFromCookie()
        {
            string culture = "fa-ir";

            if (HttpContext.Current.Request.Cookies["lang"] != null)
                culture = HttpContext.Current.Request.Cookies["lang"].Value;

            switch (culture.ToLower())
            {
                case "en-us":
                    return new CultureInfo("en-us");

                case "fa-ir":
                default:
                    return new CultureInfo("fa-ir");
            }
        }

        public static string ComputeSHA1Hash(string text)
        {
            HashAlgorithm algorithm = new SHA1Managed();

            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static Int64 GenerateUniqueOrderId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static double ConvertBytesToMegabytes(long bytes)
        {
            try
            {
                return (bytes / 1024f) / 1024f;
            }
            catch
            {
                return 0;
            }
        }

    }
}