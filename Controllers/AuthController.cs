using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesOTG_Server.Models.Http.Requests;
using NotesOTG_Server.Services.Interfaces;

namespace NotesOTG_Server.Controllers
{
    
    public class AuthController : BaseController
    {
        
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }
        
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            return Ok(await userService.Register(request));
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return Ok(await userService.Login(request));
        }

        [HttpPost("refreshTokens")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokens(RefreshTokenRequest request)
        {
            var tokenRefresh = await userService.refreshTokens(request);
            return tokenRefresh.Success ? (IActionResult) Ok(tokenRefresh) : Unauthorized();
        }
        
        [HttpGet("checkEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail()
        {
            Console.WriteLine("Got a request for email");
            return Ok(await userService.EmailCheck("tristan@gmail.com"));
        }

        [HttpGet("checkUsername")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUsername([Required]string username)
        {
            return Ok(await userService.UsernameCheck(username));
        }

        [HttpPost("socialLogin")]
        public async Task<IActionResult> SocialLogin(SocialRequest socialRequest)
        {
            return Ok(await userService.SocialLogin(socialRequest));
        }

    }
}