using System.Net.Mail;
using System.Threading.Tasks;

namespace eProducts.Helper
{
    public class EmailMessenger
    {
        public void SendEmail(string subject,string body,string email_from,string email_to)
        {
            MailMessage mail = new MailMessage(email_from, email_to);

            mail.From = new MailAddress(email_from);
            mail.Subject = subject;
            string Body = body;
            mail.Body = Body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential(email_from, "howlefcefjjmcpbc");

            smtp.EnableSsl = true;
            smtp.Send(mail);
    
        }
    }
}
