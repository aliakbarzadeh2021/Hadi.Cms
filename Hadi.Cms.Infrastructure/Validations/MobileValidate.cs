
namespace Hadi.Cms.Infrastructure.Validations
{
    public class MobileValidate
    {
        public bool CheckMobileNumber(string mobileNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(mobileNumber, "(^(09|9)[13][0-9]\\d{7}$)");
        }
    }
}
