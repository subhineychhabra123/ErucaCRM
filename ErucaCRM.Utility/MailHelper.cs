using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Utility
{
    public class MailHelper
    {

        public string RecipientName { get; set; }
        public string ToAddress { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }


        public static string EMailID = string.Empty;
        public static string EMailName = string.Empty;
        public static string SMTPHostName = string.Empty;
        public static string SMTPUserName = string.Empty;
        public static string SMTPPassword = string.Empty;
        public static bool SMTPDefaultMailing = true;
        public MailHelper()
        {

            EMailID = ReadConfiguration.EMailID;
            EMailName = ReadConfiguration.EMailName;
            SMTPHostName = ReadConfiguration.SMTPHostName;
            SMTPUserName = ReadConfiguration.SMTPUserName;
            SMTPPassword = ReadConfiguration.SMTPPassword;
            SMTPDefaultMailing=ReadConfiguration.DefaultMailing;
            

        }
        /// <summary>
        /// Sends an mail message
        /// </summary>
        /// <param name="from">Sender address</param>
        /// <param name="to">Recepient address (comma separated for multiple addresses)</param>
        /// <param name="bcc">Bcc recepient (comma separated for multiple addresses)</param>
        /// <param name="cc">Cc recepient (comma separated for multiple addresses)</param>
        /// <param name="subject">Subject of mail message</param>
        /// <param name="body">Body of mail message</param>
        public void SendMailMessage(string to, string subject, string body)
        {
            //StringBuilder strBody = new StringBuilder();
            //strBody.Append(body);

            //MailMessage mailObj = new MailMessage(
            //           "noreply@sensationsolutions.com", to, subject, strBody.ToString());
            //mailObj.IsBodyHtml = true;
            //mailObj.Priority = MailPriority.High;

            //SmtpClient SMTPServer = new SmtpClient("mail.sensationsolutions.com");
            //SMTPServer.Send(mailObj);
            if (SMTPDefaultMailing == false)
            {

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(EMailID, ReadConfiguration.EMailName);
                    mail.To.Add(to);
                    //if (!System.String.IsNullOrEmpty(ReadConfiguration.ToBcc))
                    //    mail.Bcc.Add(ReadConfiguration.ToBcc);

                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(SMTPHostName, 587);

                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    ServiceReference1.Service1Client mailService = new ServiceReference1.Service1Client();
                    mailService.SendEmail(to, subject, body, true);

                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }
        }


        //private void SendMailMessage(string to, string subject, string body)
        //{
        //    try
        //    {
        //        ServiceReference1.Service1Client mailService = new ServiceReference1.Service1Client();
        //        mailService.SendEmail(to, subject, body, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


    }
}
