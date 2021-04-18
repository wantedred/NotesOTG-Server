using System.ComponentModel.DataAnnotations;

namespace NotesOTG_Server.Models.Http.Requests
{
    public struct RegisterRequest
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "Password has to be 8 to 32 characters long" , MinimumLength = 8)]
        public string Password { get; set; }
    }
}