namespace NotesOTG_Server.Services.Data.Impl.Email.Impl
{
    public class PasswordResetTemplate : Email
    {
        private readonly string passwordToken;
        
        public PasswordResetTemplate(string sendTo, string passwordToken) : base(sendTo)
        {
            this.passwordToken = passwordToken;
            SentFrom = "support@notesotg.com";
            Subject = "Reset your Password";
        }

        public override void PrepareAndSend()
        {
            Body =
                $"Hi {SendTo}, <br>You're receiving this email because you requested a password reset for your NotesOTG account. IF you did not request this change, you can safely ignore this email." +
                $@"<br>To reset your password <a href=""http://localhost:4200/resetPassword?token={passwordToken}"">click here</a>. Or copy the following url into your browser <br><br>" +
                $"https://www.notesotg.com/resetPassword?token={passwordToken}<br><br>" +
                $"Please be advised that this link will expire in 5 days."; 
            SendEmail();
            //""http://localhost:4200/resetPassword?token={passwordToken}""
            //""https://www.notesotg.com/resetPassword?token={passwordToken}""
        }
    }
}