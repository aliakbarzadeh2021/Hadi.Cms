using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hadi.Cms.Infrastructure.Validations
{
    public class ValidationHelper
    {
        public static int GetInteger(object value, int defaultValue)
        {
            int result;
            try
            {
                result = Convert.ToInt32(value);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        public static long GetLong(object value, long defaultValue)
        {
            long result;
            try
            {
                result = Convert.ToInt64(value);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        public static string GetString(object value, string defaultValue)
        {
            string result;
            try
            {
                result = Convert.ToString(value);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidUrl(string p_strValue)
        {
            try
            {
                if (Uri.IsWellFormedUriString(p_strValue, UriKind.RelativeOrAbsolute))
                {
                    Uri l_strUri = new Uri(p_strValue);
                    return (l_strUri.Scheme == Uri.UriSchemeHttp || l_strUri.Scheme == Uri.UriSchemeHttps);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidNationalCode(string nationalCode)
        {
            if (String.IsNullOrEmpty(nationalCode))
                throw new Exception("لطفا کد ملی را صحیح وارد نمایید");

            if (nationalCode.Length != 10)
                throw new Exception("طول کد ملی باید ده کاراکتر باشد");

            var regex = new Regex(@"[^0-9] ");
            if (!regex.IsMatch(nationalCode))
                throw new Exception("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید");

            if (!Regex.IsMatch(nationalCode, @"^(?!(\d)\1{9})\d{10}$"))
                return false;

            var check = Convert.ToInt32(nationalCode.Substring(9, 1));
            var result = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(nationalCode.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            int remainder = result % 11;
            return check == (remainder < 2 ? remainder : 11 - remainder);

        }

        public static bool IsValidLegalCode(string input)
        {
            // شناسه ملی اشخاص حقوقی
            //input has 11 digits that all of them are not equal
            if (!Regex.IsMatch(input, @"^(?!(\d)\1{10})\d{11}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(10, 1));
            int dec = Convert.ToInt32(input.Substring(9, 1)) + 2;
            int[] Coef = new int[10] { 29, 27, 23, 19, 17, 29, 27, 23, 19, 17 };

            var sum = Enumerable.Range(0, 10)
                          .Select(x => (Convert.ToInt32(input.Substring(x, 1)) + dec) * Coef[x])
                          .Sum() % 11;

            return sum == check;
        }
    }
}
