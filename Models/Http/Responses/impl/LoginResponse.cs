using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesOTG_Server.Models.Http.Responses.impl
{
    public class LoginResponse : BasicResponse
    {
        
        public string Email { get; set; }

        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
        
        public IList<string> Roles { get; set; }
        
    }
}