using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Contexts;
using NotesOTG_Server.Models.Http.Responses;
using NotesOTG_Server.Services.Data.Impl.Email.Impl;

namespace NotesOTG_Server.Services
{
    public class EmailTokenService: Service<EmailVerificationToken>
    {

        private readonly UserManager<NotesUser> userManager;

        public EmailTokenService(DatabaseContext context, UserManager<NotesUser> userManager, ILogger<EmailTokenService> logger) : base(context, logger)
        {
            this.userManager = userManager;
        }

        public async Task<EmailVerificationToken> FindByToken(string verificationToken)
        {
            return await entity.SingleOrDefaultAsync(e => e.PublicToken == verificationToken);
        }

        public bool UpdateToken(EmailVerificationToken verificationToken)
        {
            var updateToken = entity.Update(verificationToken);
            return updateToken != null;
        }

        public bool RemoveToken(EmailVerificationToken verificationToken)
        {
            var tokenRemoval = entity.Remove(verificationToken);
            return tokenRemoval != null;
        }

        public async Task<EmailVerificationToken> FindByEmail(string userEmail)
        {
            return await entity.SingleOrDefaultAsync(v => v.Email == userEmail);
        }

        public async Task<bool> SaveChanges()
        {
            var save = await base.Save();
            return save;
        }

        public async Task<BasicResponse> ConfirmEmail(string emailToken)
        {
            var verificationToken = await FindByToken(emailToken);
            if (verificationToken == null || !verificationToken.IsValid())
            {
                return new BasicResponse {Success = false, Error = "Request a new email token"};
            }
            
            var user = await userManager.FindByEmailAsync(verificationToken.Email);
            if (user == null)
            {
                return new BasicResponse {Success = false, Error = "Error 3524 has been reached. Please try again."};
            }
            
            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return new BasicResponse {Success = false, Error = "Email has already been verified"};
            }
   
            var emailVerified = await userManager.ConfirmEmailAsync(user, verificationToken.InternalToken);
            if (emailVerified.Succeeded)
            {
                RemoveToken(verificationToken);
                await SaveChanges();
            }
            return new BasicResponse {Success = emailVerified.Succeeded};
        }

        public async Task<bool> CheckEmailVerified(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                //Console.WriteLine("The user Email is null");
                return false;
            }
            var user = await userManager.FindByEmailAsync(userEmail);
            //Console.WriteLine("Is the user verified? {0}", user.EmailConfirmed);
            return user?.EmailConfirmed ?? false;
        }

        /**
         * takes the users email that we want to generate a email token for
         * Returns the string of the email token or null if can't be made
         */
        public async Task<BasicResponse> GenerateEmailVerificationToken(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                //dConsole.WriteLine("The user Email is null");
                return new BasicResponse{Success = false, Error = ("Error 4732 has occured. Please try again")};
            }
            
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null || await userManager.IsEmailConfirmedAsync(user))
            {
                return new BasicResponse
                    {Success = false, Error = user == null ? "Error 4532 has occured. Please try again" : "Your email is already confirmed"};
            }
            
            var verificationToken = await FindByEmail(userEmail);
            if (verificationToken == null || !verificationToken.IsValid() || verificationToken.Email != userEmail)
            {
                if (verificationToken != null)
                {
                    //Console.WriteLine("The token is expired or the email Doesn't EQUAL");
                    RemoveToken(verificationToken);
                    await SaveChanges();   
                }
                //Console.WriteLine("verification token is null!");
                verificationToken = new EmailVerificationToken();
            }

            //Console.WriteLine("Creating the Tokens and stuff");
            var publicTokenOriginal = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var publicTokenAltered = publicTokenOriginal.Substring(0, 22).Replace("/", "_").Replace("+", "-");
            //Console.WriteLine("The new generated email token: {0}", emailToken);
            verificationToken.PublicToken = publicTokenAltered;
            verificationToken.InternalToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            verificationToken.Provided = DateTime.UtcNow;
            verificationToken.Email = userEmail;
            
            //Console.WriteLine("Updating the token!");
            UpdateToken(verificationToken);
            new EmailVerificationTemplate(user.Email, publicTokenAltered).PrepareAndSend();
            await SaveChanges();
            //Console.WriteLine("Sending the email token {0}", emailToken);
            return new BasicResponse{Success = true};
        }

    }
}