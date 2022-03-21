using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotesOTG_Server.Models
{
    public class Notes
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(14)]
        public string Title { get; set; }

        public string PublicId { get; set; }

        [Required, MinLength(3)]
        public string Body { get; set; }

        [Required, Column(TypeName = "Date")]
        public DateTime CreationDate { get; set; }

        [Required, Column(TypeName = "DateTime")]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public bool CheckList { get; set;}

        [Required, MinLength(2), MaxLength(14)]
        public string Category { get; set; }

        [Required]
        public bool CustomCategory { get; set; }

        [Required, ForeignKey("NoteUser")]
        public NotesUser UserId { get; set; }
    }
}
