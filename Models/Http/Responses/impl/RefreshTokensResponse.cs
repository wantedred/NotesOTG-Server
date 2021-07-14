namespace NotesOTG_Server.Models.Http.Responses.impl
{
    public class RefreshTokensResponse : BasicResponse
    {
        public string PrimaryToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}