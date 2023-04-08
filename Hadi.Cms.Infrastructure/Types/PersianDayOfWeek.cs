using System.ComponentModel;

namespace Hadi.Cms.Infrastructure.Types
{
    public enum PersianDayOfWeek
    {
        // Summary:
        //     Indicates Saturday.
        [Description("شنبه")]
        Saturday = 1,

        // Summary:
        //     Indicates Sunday.
        [Description("یکشنبه")]
        Sunday = 2,

        // Summary:
        //     Indicates Monday.
        [Description("دوشنبه")]
        Monday = 3,

        // Summary:
        //     Indicates Tuesday.
        [Description("سه شنبه")]
        Tuesday = 4,

        // Summary:
        //     Indicates Wednesday.
        [Description("چهارشنبه")]
        Wednesday = 5,

        // Summary:
        //     Indicates Thursday.
        [Description("پنجشنبه")]
        Thursday = 6,

        // Summary:
        //     Indicates Friday.
        [Description("جمعه")]
        Friday = 7,
    }
}
