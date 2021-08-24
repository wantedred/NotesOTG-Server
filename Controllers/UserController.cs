using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Http.Requests;
using NotesOTG_Server.Models.Http.Responses;
using System.Text;
using System.Threading.Tasks;
using NotesOTG_Server.Services;

namespace NotesOTG_Server.Controllers
{
    public class UserController : BaseController
    {

        private readonly UserManager<NotesUser> userManager;
        private readonly EmailTokenService emailTokenService;

        public UserController(UserManager<NotesUser> userManager, EmailTokenService emailTokenService)
        {
            this.userManager = userManager;
            this.emailTokenService = emailTokenService;
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<BasicResponse> ChangePassword(ChangePasswordRequest request)
        {
            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BasicResponse { Success = false, Error = "Failed to change password unknown reason." };
            }

            if (!await emailTokenService.CheckEmailVerified(request.Email))
            {
                return new BasicResponse { Success = false, Error = "Email needs to be confirmed. Please check your email"};
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

        [HttpGet("VerifiedEmail")]
        [Authorize]
        public async Task<BasicResponse> VerifiedEmail()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(email => email.Type == ClaimTypes.Email)?.Value;
            var isUserEmailVerified = await emailTokenService.CheckEmailVerified(userEmail);
            return new BasicResponse {Success = isUserEmailVerified};
        }

        [HttpGet("IssueEmailToken")]
        [Authorize]
        public async Task<BasicResponse> IssueVerificationToken()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(email => email.Type == ClaimTypes.Email)?.Value;
            var newEmailTokenResponse = await emailTokenService.GenerateEmailVerificationToken(userEmail);
            return newEmailTokenResponse;
        }

        [HttpPost("confirmEmail")]
        public async Task<BasicResponse> VerifyEmail([FromBody]string emailToken)
        {
            var emailTokenResponse = await emailTokenService.ConfirmEmail(emailToken);
            return emailTokenResponse;
        }

    }
}
