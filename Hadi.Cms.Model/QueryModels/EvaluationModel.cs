using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Model.QueryModels
{
    public class EvaluationModel
    {
        /// <summary>
        /// مدت فعالیت شرکت
        /// </summary>
        public int DurationActivity { get; set; }

        /// <summary>
        /// نوع شرکت
        /// </summary>
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// تعداد پرسنل بیمه شده
        /// </summary>
        public int InsurancedPersonelCount { get; set; }

        /// <summary>
        /// امتیاز اعتباری شرکت
        /// </summary>
        public CreditRatingOfCompany CreditRatingOfCompany { get; set; }

        /// <summary>
        /// میزان قرارداد های جاری شرکت
        /// </summary>
        public int AmountOfCurrentCompanyContract { get; set; }

        /// <summary>
        /// میانگین امتیاز اعتباری اعضای هیئت مدیره
        /// </summary>
        public AverrageCreditRatingOfBoardMembers AverrageCreditRatingOfBoardMembers { get; set; }

        /// <summary>
        /// مانده بدهی تسهیلات شرکت
        /// </summary>
        public AmountOfFacilityDebtOfCompany AmountOfFacilityDebtOfCompany { get; set; }

        /// <summary>
        /// نوع صورتهای مالی
        /// </summary>
        public TypeOfFinancialStatement TypeOfFinancialStatement { get; set; }

        /// <summary>
        /// سرمایه ثابت
        /// </summary>
        public int FixedCapital { get; set; }


        /// <summary>
        /// میزان حقوق سهام داران در سال گذشته
        /// </summary>
        public int AmountOfShareholdersRightsLastYear { get; set; }

        /// <summary>
        /// میزان حقوق سهام داران در دو سال گذشته
        /// </summary>
        public int AmountOfShareholdersRightsInLastTwoYear { get; set; }

        /// <summary>
        /// میزان دارای ثابت شرکت
        /// </summary>
        public int FixedRateOfCompany { get; set; }


        /// <summary>
        /// سود عملیاتی شرکت در سال گذشته
        /// </summary>
        public int OpertaingProfitOfCompanyLastYear { get; set; }

        /// <summary>
        /// سود عملیاتی شرکت در دو سال گذشته
        /// </summary>
        public int OpertaingProfitOfCompanyInLastTwoYear { get; set; }

        /// <summary>
        /// تاییدیه عملکرد از کارفرما
        /// </summary>
        public bool PerformanceConfirmationFromEmployer { get; set; }

        /// <summary>
        /// گواهینامه های سیستم مدیریت
        /// </summary>
        public bool ManagementSystemCertificates { get; set; }

        /// <summary>
        /// عضویت در لیست پیمانکاران شرکت ها و سازمان های معتبر
        /// </summary>
        public bool SubscribeToListReputableCompaniesAndOrganizations { get; set; }


        /// <summary>
        /// بدهی جاری
        /// </summary>
        public int CurrentDebt { get; set; }


        /// <summary>
        /// دارایی جاری
        /// </summary>
        public int CurrentAssest { get; set; }


        /// <summary>
        /// نسبت جاری
        /// </summary>
        public int CurrentRatio { get; set; }


        /// <summary>
        /// درآمد عملیاتی
        /// </summary>
        public int OperatingIncome1 { get; set; }

        /// <summary>
        /// حساب ها و اسناد دریافتی
        /// </summary>
        public int AccountAndReceivedDocuments { get; set; }

        /// <summary>
        /// دوره وصول مطالبات
        /// </summary>
        public int PeriodicalsCollection { get; set; }


        /// <summary>
        /// جمع دارایی ها
        /// </summary>
        public int TotalAssests { get; set; }

        /// <summary>
        /// جمع بدهی ها
        /// </summary>
        public int TotalDebts { get; set; }

        /// <summary>
        /// نسبت بدهی ها به دارایی ها
        /// </summary>
        public int DetToAssestRatio { get; set; }


        /// <summary>
        /// درآمد عملیاتی
        /// </summary>
        public  int OperatingIncome2 { get; set; }


        /// <summary>
        /// سود خالص
        /// </summary>
        public int NetProfit { get; set; }

        /// <summary>
        /// نسبت بازده فروش
        /// </summary>
        public int SalesReturnRatio { get; set; }

    }
}
