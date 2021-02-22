using System.Collections.Generic;
using System.Threading.Tasks;
using NotesOTG_Server.Models;

namespace NotesOTG_Server.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddUserToRole(NotesUser user, RoleType role);
        Task<bool> RemovedUserFromRole(NotesUser user, RoleType role);
        Task<bool> AddRole(RoleType role);
        Task<bool> DeleteRole(RoleType role);
        Task<string> FindRole(RoleType role);
        Task<IList<string>> GetUserRoles(NotesUser user);
    }
}