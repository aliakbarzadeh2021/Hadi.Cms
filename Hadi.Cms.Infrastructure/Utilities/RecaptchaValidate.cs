using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Hadi.Cms.Infrastructure.Utilities
{
    public class RecaptchaValidate
    {
        public static bool Check(string response)
        {
            string secret = "6LeecCITAAAAAHVQ0maUlzfeQ8MrDE8688uwPizZ";

            //Request to Google Server
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
            ("https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" + response);
            try
            {
                //Google recaptcha Response
                var valid = false;
                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CaptchaResult data = js.Deserialize<CaptchaResult>(jsonResponse);// Deserialize Json

                        valid = Convert.ToBoolean(data.Success);
                    }
                }

                return valid;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }

    public class CaptchaResult
    {
        public string Success { get; set; }
    }
}