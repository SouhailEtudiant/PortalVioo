using PortalVioo.Interface;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace PortalVioo.Repository
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(string toEmail, string subject , string password)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("souhajjri1998@gmail.com", "lcoe jwyi ldai fifw");
            // lcoe jwyi ldai fifw
            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("souhajjri1998@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>PortalVioo Mailing</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Votre nouveau mot de passe est : </p>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>"+password+ "</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
