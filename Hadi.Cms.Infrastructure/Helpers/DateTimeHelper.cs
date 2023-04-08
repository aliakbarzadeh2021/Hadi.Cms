using System;
using System.Globalization;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public static string DateTimeToString(this DateTime dateTime, bool shamsiCalender, bool withTime = true)
        {
            string dateValue = string.Empty;

            if (shamsiCalender)
            {
                PersianCalendar persianCalender = new PersianCalendar();

                dateValue = withTime
                    ? $"{dateTime.Hour:00}:{dateTime.Minute:00} {persianCalender.GetYear(dateTime):0000}/{persianCalender.GetMonth(dateTime):00}/{persianCalender.GetDayOfMonth(dateTime):00} "
                    : $"{persianCalender.GetYear(dateTime):0000}/{persianCalender.GetMonth(dateTime):00}/{persianCalender.GetDayOfMonth(dateTime):00} ";
            }
            else
            {
                dateValue = withTime
                    ? $"{dateTime.Year:0000}/{dateTime.Month:00}/{dateTime.Day:00} {dateTime.Hour:00}:{dateTime.Minute:00}"
                    : $"{dateTime.Year:0000}/{dateTime.Month:00}/{dateTime.Day:00}";
            }

            return dateValue;
        }

        public static string ToTextOfDate(this DateTime datetime, bool persian)
        {
            string result = "";

            PersianCalendar persianCalender = new PersianCalendar();
            var now = DateTime.Now;

            if (datetime >= new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
            {
                if (persian)
                {
                    result = datetime.Hour + ":" + datetime.Minute + " " + datetime.ToString("tt", CultureInfo.InvariantCulture).Replace("AM", "ق.ظ").Replace("PM", "ب.ظ");
                }
                else
                {
                    result = datetime.Hour + ":" + datetime.Minute + " " + datetime.ToString("tt", CultureInfo.InvariantCulture);
                }
            }

            var yesterday = now.AddDays(-1);
            if (datetime < new DateTime(now.Year, now.Month, now.Day, 0, 0, 0) && datetime >= new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 0, 0, 0))
            {
                if (persian)
                {
                    result = "دیروز";
                }
                else
                {
                    result = "Yesterday";
                }
            }

            var twoDayago = now.AddDays(-2);
            if (datetime < new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 0, 0, 0) && datetime >= new DateTime(twoDayago.Year, twoDayago.Month, twoDayago.Day, 0, 0, 0))
            {
                if (persian)
                {
                    result = "2 روز پیش";
                }
                else
                {
                    result = "2 day ago";
                }
            }

            var threeDayago = now.AddDays(-3);
            if (datetime < new DateTime(twoDayago.Year, twoDayago.Month, twoDayago.Day, 0, 0, 0) && datetime >= new DateTime(threeDayago.Year, threeDayago.Month, threeDayago.Day, 0, 0, 0))
            {
                if (persian)
                {
                    result = "3 روز پیش";
                }
                else
                {
                    result = "3 day ago";
                }
            }

            if (datetime < new DateTime(threeDayago.Year, threeDayago.Month, threeDayago.Day, 0, 0, 0))
            {
                if (persian)
                {
                    result = persianCalender.GetDayOfMonth(datetime) + " " + GetMonthName(datetime, persian) + GetOldYear(datetime, persian);
                }
                else
                {
                    result = datetime.Day + " " + GetMonthName(datetime, persian) + GetOldYear(datetime, persian);
                }
            }

            return result;
        }

        private static string GetOldYear(DateTime datetime, bool persian)
        {
            PersianCalendar pc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(datetime), pc.GetMonth(datetime), pc.GetDayOfMonth(datetime));
            string[] dates = pdate.Split('/');
            int year = Convert.ToInt32(dates[0]);

            string nowDate = string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(DateTime.Now), pc.GetMonth(DateTime.Now), pc.GetDayOfMonth(DateTime.Now));
            string[] nowDates = nowDate.Split('/');
            int nowYear = Convert.ToInt32(nowDates[0]);

            int yearDifference = nowYear - year;

            if (persian)
            {
                switch (yearDifference)
                {
                    case 1: return "، 1 سال پیش";
                    case 2: return "، 2 سال پیش";
                    case 3: return "، 3 سال پیش";
                    case 4: return "، 4 سال پیش‏";
                    case 5: return "، 5 سال پیش";
                    case 6: return "، 6 سال پیش";
                    case 7: return "، 7 سال پیش";
                    case 8: return "، 8 سال پیش";
                    case 9: return "، 9 سال پیش";
                    case 10: return "، 10 سال پیش";
                    case 11: return "، 11 سال پیش";
                    case 12: return "، 12 سال پیش";
                    default: return "";
                }
            }
            else
            {
                switch (yearDifference)
                {
                    case 1: return "، 1 year ago";
                    case 2: return "، 2 years ago";
                    case 3: return "، 3 years ago";
                    case 4: return "، 4 years ago";
                    case 5: return "، 5 years ago";
                    case 6: return "، 6 years ago";
                    case 7: return "، 7 years ago";
                    case 8: return "، 8 years ago";
                    case 9: return "، 9 years ago";
                    case 10: return "، 10 years ago";
                    case 11: return "، 11 years ago";
                    case 12: return "، 12 years ago";
                    default: return "";
                }
            }
        }

        public static int GetPersianMonth(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);

            return month;
        }

        public static string GetPersianDayOfWeekName(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "يکشنبه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Tuesday: return "سه‏ شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
                case DayOfWeek.Thursday: return "پنجشنبه";
                case DayOfWeek.Friday: return "جمعه";
                default: return "";
            }
        }

        public static string GetPersianMonthName(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }
        }

        private static string GetMonthName(DateTime date, bool persian)
        {
            string fullMonthName = "";
            if (persian)
            {
                //fullMonthName = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("fa-ir")); //for windows that contain persian calender
                fullMonthName = date.GetPersianMonthName();
            }
            else
            {
                fullMonthName = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("en-us"));
            }
            return fullMonthName;
        }

        public static string DateTimeToString(DateTime? dateTime, bool shamsiCalender = false)
        {
            string dateValue = string.Empty;

            if (dateTime.HasValue)
            {
                if (shamsiCalender)
                {
                    PersianCalendar persianCalender = new PersianCalendar();

                    dateValue =
                        $"{dateTime.Value.Hour:00}:{dateTime.Value.Minute:00} {persianCalender.GetYear(dateTime.Value):0000}/{persianCalender.GetMonth(dateTime.Value):00}/{persianCalender.GetDayOfMonth(dateTime.Value):00} ";
                }
                else
                {
                    dateValue =
                        $"{dateTime.Value.Year:0000}/{dateTime.Value.Month:00}/{dateTime.Value.Day:00} {dateTime.Value.Hour:00}:{dateTime.Value.Minute:00}";
                }
            }

            return dateValue;
        }
        public static string GetCurrentYear(DateTime? dateTime, bool shamsiCalender = false)
        {
            string dateValue = string.Empty;

            if (dateTime.HasValue)
            {
                if (shamsiCalender)
                {
                    PersianCalendar persianCalender = new PersianCalendar();

                    dateValue = $"{persianCalender.GetYear(dateTime.Value):0000}";
                }
                else
                {
                    dateValue = dateTime.Value.Year.ToString();
                }
            }

            return dateValue;
        }


        public static string ToGregorian(this DateTime date)
        {
            string shamsiDate = date.ToString("yyyy/MM/dd");
            int year = int.Parse(shamsiDate.Substring(0, 4));
            int month = int.Parse(shamsiDate.Substring(5, 2));
            int day = int.Parse(shamsiDate.Substring(8, 2));
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            string xyear = dt.Year.ToString();
            string xmonth = dt.Month.ToString();
            if (xmonth.ToString().Length == 1)
            {
                xmonth = "0" + xmonth;
            }
            string xday = dt.Day.ToString();
            if (xday.ToString().Length == 1)
            {
                xday = "0" + xday;
            }
            return xyear + "/" + xmonth + "/" + xday;
        }

        public static string ToPersianDate(this DateTime date)
        {
           return $"{new PersianCalendar().GetDayOfMonth(date)} {date.GetPersianMonthName()} {GetCurrentYear(date, true)}";
        }
    }
}
