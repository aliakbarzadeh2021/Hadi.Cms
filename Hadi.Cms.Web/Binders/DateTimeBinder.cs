using Hadi.Cms.Web.Utilities;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Binders
{
    public class DateTimeBinder : IModelBinder
    {
        public static void RegisterBinder(ModelBinderDictionary binders)
        {
            if (!binders.ContainsKey(typeof(DateTime)))
                binders.Add(typeof(DateTime), new DateTimeBinder());

            if (!binders.ContainsKey(typeof(DateTime?)))
                binders.Add(typeof(DateTime?), new DateTimeBinder());
        }

        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult providerResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            DateTime? result = null; //to be set
            bool valueBoundSuccessfullyFa = false;

            if (providerResult != null)
            {
                valueBoundSuccessfullyFa = true;

                if (providerResult.AttemptedValue.IndexOf(" ") >= 0)
                {
                    string datePart = providerResult.AttemptedValue.Split(' ')[0];
                    string timePart = providerResult.AttemptedValue.Split(' ')[1];

                    DateTime dateValue;
                    TimeSpan timeValue;

                    if (ParseDate(datePart, out dateValue) && ParseTime(timePart, out timeValue))
                        result = dateValue + timeValue;
                    else
                        valueBoundSuccessfullyFa = false;
                }

                else
                {
                    DateTime dateValue;

                    if (ParseDate(providerResult.AttemptedValue, out dateValue))
                        result = dateValue;
                    else
                        valueBoundSuccessfullyFa = false;
                }
            }

            if (bindingContext.ModelType == typeof(DateTime) && !result.HasValue)
                throw new ArgumentException(bindingContext.ModelName);

            if (bindingContext.ModelType == typeof(DateTime))
                return result.Value;

            else if (bindingContext.ModelType == typeof(DateTime?))
                return result;

            else
                throw new NotImplementedException();
        }

        private static bool ParseTime(string timeValue, out TimeSpan timeOfDay)
        {
            return TimeSpan.TryParse(timeValue, out timeOfDay);
        }
        private static bool ParseDate(string dateValue, out DateTime date)
        {
            date = new DateTime();

            string[] parts = dateValue.Split('/');

            if (parts.Length != 3)
                return false;

            else
            {
                int y, m, d;

                if (!int.TryParse(parts[0], out y) || !int.TryParse(parts[1], out m) || !int.TryParse(parts[2], out d))
                    return false;
                else
                {
                    if (SessionData.Current.HasRightToLeftUI)
                        date = new PersianCalendar().ToDateTime(y, m, d, 0, 0, 0, 0);
                    else
                        date = new DateTime(y, m, d);

                    return true;
                }
            }
        }
    }
}