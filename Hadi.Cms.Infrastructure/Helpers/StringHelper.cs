using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class StringHelper
    {
        private static readonly string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        private static readonly string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static string ToPersianNumber(this string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(en[i], pn[i]);
            return chash;
        }

        public static string ToEnglishNumber(this int intNum)
        {
            string chash = intNum.ToString();
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }

        public static string ToEnglishNumber(this string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }

        public static string LimitLength(this string str, int charAllowed, string continueWith)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length > charAllowed)
                return str.Substring(0, charAllowed) + " " + continueWith;
            return str;
        }

        public static string ConvertToHtml(this string text)
        {
            return text.Replace("\r\n", "<br />").Replace("\"", "&quot;").Replace("#", "&#35").Replace("$", "&#36")
                .Replace("%", "&#37").Replace("&", "&amp;").Replace("'", "&#39").Replace("(", "&#40").Replace(")", "&#41")
                .Replace("*", "&#42").Replace("+", "&#43").Replace(",", "&#44").Replace("-", "&#45").Replace(".", "&#46").Replace("/", "&#47");
        }

        public static DateTime ToGregorianDate(this string persianDate)
        {
            DateTime dt;
            persianDate = persianDate.Substring(0, 10);
            if (persianDate.IndexOf('/') == 4)
            {
                var year = int.Parse(persianDate.Split('/')[0]);
                var month = int.Parse(persianDate.Split('/')[1]);
                var day = int.Parse(persianDate.Split('/')[2]);
                dt = new DateTime(year, month, day, new PersianCalendar());
            }
            else
            {
                var year = int.Parse(persianDate.Split('/')[2]);
                var month = int.Parse(persianDate.Split('/')[1]);
                var day = int.Parse(persianDate.Split('/')[0]);
                dt = new DateTime(year, month, day, new PersianCalendar());
            }

            GregorianCalendar pc = new GregorianCalendar();
            return pc.ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
        }

        public static string ToGregorianDate(this string persianDate, bool asString)
        {
            DateTime dt;
            persianDate = persianDate.Substring(0, 10);
            if (persianDate.IndexOf('/') == 4)
            {
                var year = int.Parse(persianDate.Split('/')[0]);
                var month = int.Parse(persianDate.Split('/')[1]);
                var day = int.Parse(persianDate.Split('/')[2]);
                dt = new DateTime(year, month, day, new PersianCalendar());
            }
            else
            {
                var year = int.Parse(persianDate.Split('/')[2]);
                var month = int.Parse(persianDate.Split('/')[1]);
                var day = int.Parse(persianDate.Split('/')[0]);
                dt = new DateTime(year, month, day, new PersianCalendar());
            }

            var result = dt.Month + "/" + dt.Day + "/" + dt.Year;
            return result;
        }

        public static string ToFormattedThousands(this string number)
        {
            return Convert.ToInt32(number).ToString("N0");
        }

        public static string PersianToEnglishNumber(this string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                if (char.IsDigit(item))
                    if(LettersDictionary.ContainsKey(item))
                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            return persianStr;
        }

        public static string BetweenStrings(string text, string start, string end)
        {
            var part1 = text.IndexOf(start) + start.Length;
            var part2 = text.IndexOf(end, part1);
            
            if (end == "") return (text.Substring(part1));
            
            return text.Substring(part1, part2 - part1);
        }



    }
}
