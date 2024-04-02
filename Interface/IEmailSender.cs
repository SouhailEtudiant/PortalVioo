namespace PortalVioo.Interface
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, string subject, string password);
    }
}
