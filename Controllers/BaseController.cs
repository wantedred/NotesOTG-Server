using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Contexts;

namespace NotesOTG_Server.Controllers
{
    [Route("API/[Controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        { }
    }
}