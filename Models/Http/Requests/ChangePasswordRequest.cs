using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesOTG_Server.Models.Http.Requests
{
    public struct ChangePasswordRequest
    {

        public string Email { get; set; }
        public string CurrentPassword { get; set;  }
        public string NewPassword { get; set; }

    }
}
