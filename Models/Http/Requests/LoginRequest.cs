using System.ComponentModel.DataAnnotations;

namespace NotesOTG_Server.Models.Http.Requests
{
    public struct LoginRequest
    {
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}