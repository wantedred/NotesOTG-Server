using System.ComponentModel.DataAnnotations;

namespace NotesOTG_Server.Models.Http.Requests
{
    public struct RefreshTokenRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string RefreshToken { get; set; }
    }
}