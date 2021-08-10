using NotesOTG_Server.Services.Background;

namespace NotesOTG_Server.Services.Data.Impl.Email.Impl
{
    public class EmailVerificationTemplate : Email
    {

        private readonly string emailToken;

        public EmailVerificationTemplate(string @sendTo, string emailToken) : base(@sendTo)
        {
            this.emailToken = emailToken;
            SentFrom = "support@notesotg.com";
            Subject = "Please verify your email";
        }

        public override void PrepareAndSend()
        {
            Body = $@"Hi {SendTo}, <br>Thank you signing up for NotesOTG. 
                To verify your email address <a href=""https://www.notesotg.com/verifyemail?token={emailToken}"">Click here</a>
                or copy the following url into your browser: <br><br>
                https://www.notesotg.com/verifyemail?token={emailToken}
                <br><br>Please be advised that this link will expire in 5 days.";
            SendEmail();
        }
        
    }
}