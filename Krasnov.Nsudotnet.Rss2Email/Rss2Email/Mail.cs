using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Rss2Email
{
    class Mail
    {

        public static void SendMail(string news, string receiver)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("coilprog", "lim0nishe");  //фейковая почта на гмале

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("coilprog@gmail.com");  
                mail.To.Add(new MailAddress(receiver));
                mail.Subject = "New RSS news";
                mail.Body = news;

                smtp.Send(mail);

                mail.Dispose();
                smtp.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
          
        }
    }
}
