using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebApp.Models
{
    public class MailHelper
    {
        public static void Send(string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage("coajovic.web@gmail.com", to, subject, body);
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("coajovic.web@gmail.com", "tastatura14");
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                string a = e.Message;
            }
        }
    }
}