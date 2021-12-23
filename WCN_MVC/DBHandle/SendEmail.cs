using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WCN_MVC.DBHandle
{
    public class SendEmail
    {
        public int SendMail_GMail(string mailTo, string mailSubject, string mailBody, string mailFrom)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            int rtnMsg = 0;

            try
            {
                MailAddress fromAddress = new MailAddress(mailFrom);
                message.From = fromAddress;
                message.To.Add(mailTo);

                message.Subject = mailSubject;
                message.IsBodyHtml = true;

                message.Body = mailBody;

                smtpClient.Host = "smtp.gmail.com";   // We use gmail as our smtp client
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("wcn.togs@gmail.com", "Togs@1234");

                smtpClient.Send(message);
                rtnMsg = 1;
                return rtnMsg;
            }
            catch (Exception ex)
            {
                rtnMsg = 2;
                return rtnMsg;
            }
            finally
            { }
        }
    }
}