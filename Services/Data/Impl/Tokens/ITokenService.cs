using System.Threading.Tasks;
using NotesOTG_Server.Models;

namespace NotesOTG_Server.Services.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshToken> FindByToken(string refreshToken);
        bool UpdateToken(RefreshToken refreshToken);
        bool RemoveToken(RefreshToken refreshToken);
        Task<bool> SaveChanges();
        string GeneratePrimaryToken(string userId);

        Task<string> IssueStandardRefresh(string refreshToken, string email);
        Task<string> IssueEmailRefresh(string email);
    }
}