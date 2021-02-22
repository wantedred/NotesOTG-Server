namespace NotesOTG_Server.Models.Http.Responses.impl
{
    public class RegisterResponse : BasicResponse
    {
        public string DisplayNameError { get; set; }
        
        public string EmailError { get; set; }

        public string PasswordError { get; set; }
    }
}