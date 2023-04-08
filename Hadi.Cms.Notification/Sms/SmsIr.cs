using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Hadi.Cms.Notification.Sms
{
    public class ConfigurationSmsIr
    {
        public static string Username = "9125554979";
        public static string Password = "842220931";
        public static string LineNumber = "50004465000003";
        public static string UserApiKey = "adc0c4ce2f5ec0c1be01465";
        public static string SecretKey = "T@gh@v!Cms2010";
    }

    public class SmsIr
    {
        public bool SendMessage(string smsTo, string smsText)
        {
            try
            {
                const string uri = "http://ip.sms.ir/SendMessage.ashx";
                var myParameters = HttpUtility.UrlPathEncode(
                    $"user={ConfigurationSmsIr.Username}&pass={ConfigurationSmsIr.Password}&lineNo={ConfigurationSmsIr.LineNumber}&to={smsTo}&text={smsText}");

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.UploadString(uri, myParameters);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //install nuget package https://www.nuget.org/packages/SmsIrRestful/
        public bool SendMessageRestful(string smsTo, string smsText)
        {
            try
            {
                var token = new Token().GetToken(ConfigurationSmsIr.UserApiKey, ConfigurationSmsIr.SecretKey);

                var messageSendObject = new MessageSendObject()
                {
                    Messages = new List<string> { smsText }.ToArray(),
                    MobileNumbers = new List<string> { smsTo }.ToArray(),
                    LineNumber = ConfigurationSmsIr.LineNumber,
                    SendDateTime = DateTime.Now,
                    CanContinueInCaseOfError = true
                };

                var messageSendResponseObject = new MessageSend().Send(token, messageSendObject);

                if (messageSendResponseObject.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SendVerificationCodeRestful(string mobileNumber, string code)
        {
            try
            {
                var token = new Token().GetToken(ConfigurationSmsIr.UserApiKey, ConfigurationSmsIr.SecretKey);

                var restVerificationCode = new RestVerificationCode()
                {
                    Code = code,
                    MobileNumber = mobileNumber
                };

                var restVerificationCodeRespone = new VerificationCode().Send(token, restVerificationCode);

                if (restVerificationCodeRespone.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool FastSendMessageRestful(long mobileNumber, int messageTemplateId, Dictionary<string, string> parameters)
        {
            try
            {
                var token = new Token().GetToken(ConfigurationSmsIr.UserApiKey, ConfigurationSmsIr.SecretKey);

                var ultraFastSend = new UltraFastSend()
                {
                    Mobile = mobileNumber,
                    TemplateId = messageTemplateId
                };

                var ultraFastParameters = new List<UltraFastParameters>();
                foreach (var key in parameters.Keys)
                {
                    ultraFastParameters.Add(new UltraFastParameters()
                    {
                        Parameter = key,
                        ParameterValue = parameters[key]
                    });
                }
                ultraFastSend.ParameterArray = ultraFastParameters.ToArray();

                UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

                if (ultraFastSendRespone.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

