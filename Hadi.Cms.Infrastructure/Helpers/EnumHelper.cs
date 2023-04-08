using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public class EnumHelper
    {
        public static SelectList GetEnumSelectList<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = GetDisplayForEnumValue(e) };

            return new SelectList(values, "Id", "Name");
        }

        public static SelectList GetEnumSelectList<TEnum>(TEnum selectedValue) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = GetDisplayForEnumValue(e) };

            return new SelectList(values, "Id", "Name", selectedValue);
        }

        public static string GetDisplayForEnumValue(object value)
        {
            var enumType = value.GetType();
            var field = enumType.GetFields()
                .First(x => x.Name == Enum.GetName(enumType, value));

            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute != null)
                return displayAttribute.GetName();

            return value.ToString();
        }
    }
}
