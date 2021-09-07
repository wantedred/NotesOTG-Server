using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Contexts;
using NotesOTG_Server.Models.Http.Responses;
using NotesOTG_Server.Services.Data.Impl.Email.Impl;

namespace NotesOTG_Server.Services.Data.Impl.Tokens
{
    public class PasswordTokenService : Service<PasswordVerificationToken>
    {

        private readonly UserManager<NotesUser> userManager;

        protected PasswordTokenService(DatabaseContext context, ILogger<Service<PasswordVerificationToken>> logger, UserManager<NotesUser> userManager) : base(context, logger)
        {
            this.userManager = userManager;
        }
        
        public async Task<PasswordVerificationToken> FindByToken(string verificationToken)
        {
            return await entity.SingleOrDefaultAsync(e => e.PublicToken == verificationToken);
        }

        public bool UpdateToken(PasswordVerificationToken verificationToken)
        {
            var updateToken = entity.Update(verificationToken);
            return updateToken != null;
        }

        public bool RemoveToken(PasswordVerificationToken verificationToken)
        {
            var tokenRemoval = entity.Remove(verificationToken);
            return tokenRemoval != null;
        }

        public async Task<PasswordVerificationToken> FindByEmail(string userEmail)
        {
            return await entity.SingleOrDefaultAsync(v => v.Email == userEmail);
        }

        public async Task<bool> SaveChanges()
        {
            var save = await base.Save();
            return save;
        }
        //TODO: Work on actually generating reset email and token | done
        //TODO: confirm if token is good, along with new password | done
        //TODO: thing of how password and email will integrate
        //TODO: test if i can reset password and then confirm email?
        
        //TODO: Work on angular password reset. Login have a reset password link
        //TODO: On angular have it check if token is valid first, and then allow password typing
        //TODO: After the password is confirmed on change. Relocate user to login again (5 seconds)
        public async Task<BasicResponse> GenerateAndSendPasswordToken(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return new BasicResponse {Success = false, Error = "Error 2142 has occured. Please try again."};
            }
            
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new BasicResponse {Success = false, Error = "Error 2145 has occured. Please try again."};
            }

            var passwordResetToken = await FindByEmail(userEmail);
            if (passwordResetToken == null || !passwordResetToken.IsValid())
            {
                if (passwordResetToken != null)
                {
                    RemoveToken(passwordResetToken);
                }

                passwordResetToken = new PasswordVerificationToken();
            }
            
            var publicTokenOriginal = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var publicTokenAltered = publicTokenOriginal.Substring(0, 22).Replace("/", "_").Replace("+", "-");
            //Console.WriteLine("The new generated email token: {0}", emailToken);
            passwordResetToken.PublicToken = publicTokenAltered;
            passwordResetToken.InternalToken = await userManager.GeneratePasswordResetTokenAsync(user);
            passwordResetToken.Provided = DateTime.UtcNow;
            passwordResetToken.Email = userEmail;

            UpdateToken(passwordResetToken);
            new PasswordResetTemplate(userEmail, publicTokenAltered).PrepareAndSend();
            await SaveChanges();
            return new BasicResponse {Success = true};
        }

        public async Task<BasicResponse> VerifyPasswordToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new BasicResponse {Success = false, Error = "Error 2140 has occured. Please try again."};
            }

            var passwordToken = await FindByToken(token);
            return new BasicResponse {Success = passwordToken != null};
        }

        public async Task<BasicResponse> PasswordResetWithToken(string token, string password)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(password))
            {
                return new BasicResponse {Success = false, Error = string.IsNullOrEmpty(password) ? 
                    "Error 2138 has occured. Please try again": "Error 2139 has occured. Please try again"};
            }

            var passwordToken = await FindByToken(token);
            var user = await userManager.FindByEmailAsync(passwordToken?.Email);
            if (user == null || passwordToken == null)
            {
                return new BasicResponse {Success = false, Error = user == null ? 
                    "Error 2141 has occured. Please try again": "Error 2142 has occured. Please try again"};
            }
            var resetProcess = await userManager.ResetPasswordAsync(user, passwordToken.InternalToken, password);
            if (resetProcess.Succeeded)
            {
                RemoveToken(passwordToken);
                return new BasicResponse {Success = true};
            }
            var errors = new StringBuilder();
            foreach (var error in resetProcess.Errors)
            {
                errors.Append(error.Description);
                errors.Append(":");
            }
            return new BasicResponse {Success = resetProcess.Succeeded, Error = errors.ToString(0, errors.Length - 1)};
        }
    }
}