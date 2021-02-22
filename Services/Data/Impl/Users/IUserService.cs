using System.Threading.Tasks;
using NotesOTG_Server.Models.Http.Requests;
using NotesOTG_Server.Models.Http.Responses;
using NotesOTG_Server.Models.Http.Responses.impl;

namespace NotesOTG_Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<RegisterResponse> Register(RegisterRequest request);
        Task<BasicResponse> EmailCheck(string email);
        Task<BasicResponse> UsernameCheck(string username);
        Task<RefreshTokensResponse> refreshTokens(RefreshTokenRequest request);
    }
}