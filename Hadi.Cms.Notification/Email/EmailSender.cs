using Hadi.Cms.Log.EventLoger;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hadi.Cms.Notification.Email
{
    public class ConfigurationSMTP
    {
        public static string smtpAdress = "194.146.239.90";
        public static int portNumber = 25;
        public static bool enableSSL = false;
        public static string from = "info@park-other.ir";
        public static string password = "fbxhGutlcvo7swegidap";
    }

    public class EmailProvider
    {
        private EventLoger _eventLoger;
        public EmailProvider()
        {
            _eventLoger = new EventLoger();
        }

        public bool SendMail(string emailTo, string subject, string message, bool withAttachment = false, string attachmentFilePath = "")
        {
            try
            {
                string smtpAddress = ConfigurationSMTP.smtpAdress;
                int portNumber = ConfigurationSMTP.portNumber;   //Smtp port
                bool enableSSL = ConfigurationSMTP.enableSSL;  //SSL enable

                StringBuilder body = new StringBuilder();

                body.Append("<html style='style='direction: rtl;'><head></head><body style='direction: rtl;'");
                body.Append("<div style=' font-family: Tahoma; font - size: 14px; color: black; '><br>");
                body.Append(message);
                body.Append("</div>");
                body.Append("</body></html>");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(ConfigurationSMTP.from);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body.ToString();
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    if (withAttachment)
                    {
                        mail.Attachments.Add(new Attachment(attachmentFilePath));
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(ConfigurationSMTP.from, ConfigurationSMTP.password);
                        //Authentication required
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _eventLoger.LogException("EmailProvider - SendMail", "Send New Message Failed", ex);
                return false;
            }
        }
    }
}
