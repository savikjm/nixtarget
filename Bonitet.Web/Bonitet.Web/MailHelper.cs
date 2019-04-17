using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Bonitet.Web
{
    public class MailHelper
    {
        public static void SendMail(string SendTo, string Subject, string Message, bool IsHtml)
        {
            MailMessage mailMessage = new MailMessage();

            //mailMessage.From = new MailAddress(SendFrom);


            var recepients = SendTo.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in recepients)
            {
                if(string.IsNullOrEmpty(r) == false)
                    mailMessage.To.Add(new MailAddress(r.Trim()));
            }
            

            mailMessage.Body = Message;

            mailMessage.Subject = Subject;

            mailMessage.IsBodyHtml = IsHtml;

            SmtpClient smtpClient = new SmtpClient();
            //smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex) {
                var test = ex;
            }
        }


        public static void SendMailToAdmin(string Subject, string Message, bool IsHtml)
        {
            var adminEmail = ConfigurationSettings.AppSettings["defaultEmail"];

            SendMail(adminEmail, Subject, Message, IsHtml);
        }
    }
}