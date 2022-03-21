using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesOTG_Server.Models.Http.Requests
{
    public struct NotesRequest
    {

        public int Id;

        [Required, MinLength(3), MaxLength(14)]
        public string Title { get; set; }

        [Required, MinLength(3)]
        public string Body { get; set; }

        public string PublicId { get; set; }

        [Required]
        public string CreationDate { get; set; }

        [Required]
        public string ModifiedDate { get; set; }

        [Required]
        public bool CheckList { get; set; }

        [Required, MinLength(2), MaxLength(14)]
        public string Category { get; set; }

        [Required]
        public bool CustomCategory { get; set; }

    }
}
