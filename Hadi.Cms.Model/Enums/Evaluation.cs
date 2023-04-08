using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Enums
{
    /// <summary>
    /// مدت زمان فعالیت
    /// </summary>
    public enum DurationActivity
    {
        [Display(Name = "کمتر از 1 سال")]
        LessThanOneYear = 1,

        [Display(Name = "1 تا 3 سال")]
        OneToTreeYear = 2,

        [Display(Name = "۳ سال یا بیشتر")]
        TreeYearOrMore = 3
    }


    /// <summary>
    /// نوع شرکت
    /// </summary>
    public enum CompanyType
    {
        [Display(Name = "مسئولیت محدود")]
        LimitedResponsibility = 1,

        [Display(Name = "تعاونی")]
        Cooperative = 2,

        [Display(Name = "سهامی خاص")]
        PrivateEquity = 3
    }

    /// <summary>
    /// امتیاز اعتباری شرکت
    /// </summary>
    public enum CreditRatingOfCompany
    {
        [Display(Name = "A+")]
        APlus = 1,

        [Display(Name = "A")]
        A = 2,

        [Display(Name = "A-")]
        AMinus = 3,

        [Display(Name = "B+")]
        BPlus = 4,

        [Display(Name = "B")]
        B = 5,

        [Display(Name = "B-")]
        BMinus = 6,

        [Display(Name = "C+")]
        CPlus = 7,

        [Display(Name = "C")]
        C = 8,

        [Display(Name = "C-")]
        CMinus = 9

    }


    /// <summary>
    /// میانگین امتیاز اعتباری اعضای هیئت مدیره
    /// </summary>
    public enum AverrageCreditRatingOfBoardMembers
    {
        [Display(Name = "A+")]
        APlus = 1,

        [Display(Name = "A")]
        A = 2,

        [Display(Name = "A-")]
        AMinus = 3,

        [Display(Name = "B+")]
        BPlus = 4,

        [Display(Name = "B")]
        B = 5,

        [Display(Name = "B-")]
        BMinus = 6,

        [Display(Name = "C+")]
        CPlus = 7,

        [Display(Name = "C")]
        C = 8,

        [Display(Name = "C-")]
        CMinus = 9
    }


    /// <summary>
    /// مانده بدهی تسهیلات شرکت
    /// </summary>
    public enum AmountOfFacilityDebtOfCompany
    {
        [Display(Name = "کمتر از 1 سال")]
        LessThanOneYear = 1,

        [Display(Name = "1 تا 3 سال")]
        OneToTreeYear = 2,

        [Display(Name = "۳ سال یا بیشتر")]
        TreeYearOrMore = 3
    }


    /// <summary>
    /// نوع صورتهای مالی
    /// </summary>
    public enum TypeOfFinancialStatement
    {
        [Display(Name = "فاقد اظهارنامه مالی در طی دو سال گذشته")]
        NoFinancialStatementOverPastTwoYears = 1,

        [Display(Name = "دارای اظهارنامه مالی در سال گذشته")]
        HasFinancialStatementLastYear = 2,

        [Display(Name = "دارای اظهارنامه مالی در سال گذشته")]
        HasFinancialStatementForTwoPastYears = 3,

        [Display(Name = "دارای صورت های مالی حسابرسی شده")]
        HasAuditedFinancialStatements = 4

    }




}
