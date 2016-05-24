using System;
using System.Net;
using System.Net.Mail;

namespace Rss2Email
{
    class Mail
    {

        public static void SendMail(string news, string receiver)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("coilprog", "lim0nishe"); //фейковая почта на гмале

                   using( MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("coilprog@gmail.com");
                        mail.To.Add(new MailAddress(receiver));
                        mail.Subject = "New RSS news";
                        mail.Body = news;

                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
          
        }
    }
}
