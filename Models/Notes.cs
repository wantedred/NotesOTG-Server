using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesOTG_Server.Models
{
    public class Notes
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool CheckList { get; set;}

        public string Category { get; set; }

        public bool CustomCategory { get; set; }

        public NotesUser UserId { get; set; }

    }
}
