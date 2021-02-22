using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NotesOTG_Server.Models;
using NotesOTG_Server.Models.Http.Requests;
using NotesOTG_Server.Models.Http.Responses;
using NotesOTG_Server.Models.Http.Responses.impl;
using NotesOTG_Server.Services.Interfaces;

namespace NotesOTG_Server.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<NotesUser> userManager;
        private readonly SignInManager<NotesUser> signInManager;
        private readonly IRoleService roleService;
        private readonly ITokenService tokenService;

        public UserService ( UserManager<NotesUser> userManager,
            SignInManager<NotesUser> signInManager,
            IRoleService roleService,
            ITokenService tokenService) 
        {
            this.userManager = userManager;
            this.roleService = roleService;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
        }
        
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponse {Success = false, Error = "Invalid email or password."};
            }

            var loginResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!loginResult.Succeeded)
            {
                return new LoginResponse {Success = false, Error = loginResult.IsLockedOut ? 
                    "This account is currently locked. Please check your email." 
                    : "Invalid email or password."};
            }
            
            var token = tokenService.GeneratePrimaryToken(user.Id);
            var refreshToken = await tokenService.IssueEmailRefresh(user.Email);
            var roles = await roleService.GetUserRoles(user);
            return new LoginResponse {Success = true, Email = user.Email, Username = user.UserName, Token = token, RefreshToken = refreshToken, Roles = roles};
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var notesUser = new NotesUser()
            {
                UserName = request.DisplayName,
                Email = request.Email
            };

            var addUserTask = await userManager.CreateAsync(notesUser, request.Password);
            if (addUserTask.Succeeded)
            {
                await roleService.AddUserToRole(notesUser, RoleType.User);
                return new RegisterResponse {Success = true};
            }
            
            var errors = new StringBuilder();
            foreach (var error in addUserTask.Errors)
            {
                errors.Append(error.Description);
                errors.Append(":");
            }
            return new RegisterResponse { Success = false, Error = errors.ToString(0, errors.Length - 1)};
        }

        public async Task<RefreshTokensResponse> refreshTokens(RefreshTokenRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new RefreshTokensResponse { Success = false };
            }

            var refreshToken = await tokenService.IssueStandardRefresh(request.RefreshToken, request.Email);
            var token = tokenService.GeneratePrimaryToken(user.Id);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new RefreshTokensResponse { Success = false };
            }
            return new RefreshTokensResponse { Success = true, Token = token, RefreshToken = refreshToken };
        }
        
        public async Task<BasicResponse> EmailCheck(string email)
        {
            var emailExists = await userManager.FindByEmailAsync(email);
            return emailExists == null ? new BasicResponse{Success = true} 
                : new BasicResponse{Success = false, Error = "Email is already in use."};
        }

        public async Task<BasicResponse> UsernameCheck(string username)
        {
            var usernameExists = await userManager.FindByNameAsync(username);
            return usernameExists == null ? new BasicResponse{Success = true} 
                : new BasicResponse{Success = false, Error = "Username is already in use."};
        }
        
    }
}