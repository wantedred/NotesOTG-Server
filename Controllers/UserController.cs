using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Http.Requests;
using NotesOTG_Server.Models.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesOTG_Server.Controllers
{
    public class UserController : BaseController
    {

        public UserManager<NotesUser> userManager;

        public UserController(UserManager<NotesUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<BasicResponse> ChangePassword(ChangePasswordRequest request)
        {
            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BasicResponse { Success = true };
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            var changePassword = hasPassword ? 
                await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword) :
                await userManager.AddPasswordAsync(user, request.NewPassword);

            var errors = new StringBuilder();
            foreach (var error in changePassword.Errors)
            {
                errors.Append(error.Description);
                errors.Append(":");
            }

            return changePassword.Succeeded ?
                new BasicResponse { Success = true } :
                new BasicResponse { Success = false, Error = errors.ToString(0, errors.Length - 1) };
        }

    }
}
