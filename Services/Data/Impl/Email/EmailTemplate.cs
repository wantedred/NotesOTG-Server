using MimeKit;
using MimeKit.Text;
using NotesOTG_Server.Services.Background;

namespace NotesOTG_Server.Services.Data.Impl.Email
{
    public abstract class Email
    {
        protected string Subject { get; set; }
        protected string Body { get; set; }
        protected string SendTo { get; set; }
        protected string SentFrom { get; set; }
        
        private const string Header = @"<div style=""border: 1px solid #cccccc; padding: 10px; margin: 0 auto; max-width: 680px; background-color: #fff;"">";
        private const string Footer = @"</div>";

        protected Email(string sendTo)
        {
            SendTo = sendTo;
        }
        
        public abstract void PrepareAndSend();

        protected void SendEmail()
        {
            var mailMessage = new MimeMessage();
            var body = Header + Body + Footer;
            mailMessage.From.Add(MailboxAddress.Parse(SentFrom));
            mailMessage.To.Add(MailboxAddress.Parse(SendTo));
            mailMessage.Subject = Subject;
            mailMessage.Body = new TextPart(TextFormat.Html) {Text = body};
            TimedEmailService.PendingEmails.Add(mailMessage);
        }

    }
}