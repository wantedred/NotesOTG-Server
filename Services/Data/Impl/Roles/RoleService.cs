using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NotesOTG_Server.Models;
using NotesOTG_Server.Services.Interfaces;

namespace NotesOTG_Server.Services
{
    public class RoleService
    {

        private readonly UserManager<NotesUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        public RoleService(UserManager<NotesUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> AddUserToRole(NotesUser user, RoleType role)
        {
            IdentityResult userToRole;
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                var roleAdd = await AddRole(role);
                if (roleAdd)
                {
                    userToRole = await userManager.AddToRoleAsync(user, role.ToString());
                }
                return false;
            }
            userToRole = await userManager.AddToRoleAsync(user, role.ToString());
            return userToRole.Succeeded;
        }

        public async Task<bool> RemovedUserFromRole(NotesUser user, RoleType role)
        {
            var removedUserRole = await userManager.RemoveFromRoleAsync(user, role.ToString()); 
            return removedUserRole.Succeeded;
        }

        public async Task<bool> AddRole(RoleType role)
        {
            var roleAdd = await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            return roleAdd.Succeeded;
        }

        public async Task<bool> DeleteRole(RoleType role)
        {
            var deleteRole = await roleManager.DeleteAsync(new IdentityRole(role.ToString()));
            return deleteRole.Succeeded;
        }

        public async Task<string> FindRole(RoleType role)
        {
            var roleFind = await roleManager.FindByNameAsync(role.ToString());
            return roleFind.Name;
        }

        public async Task<IList<string>> GetUserRoles(NotesUser user)
        {
            var roleFind = await userManager.GetRolesAsync(user);
            return roleFind;
        }
    }
}