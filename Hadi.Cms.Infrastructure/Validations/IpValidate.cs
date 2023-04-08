using System.Linq;

namespace Hadi.Cms.Infrastructure.Validations
{
    public class IpValidate
    {
        public static bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            var splitValues = ipString.Split('.');
            return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out _));
        }
    }
}
