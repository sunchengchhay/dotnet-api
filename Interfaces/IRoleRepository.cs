using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
        Task<List<string>> GetUserRolesAsync(string emailId);
        Task<List<string>> AddRolesAsync(string[] roles);
        Task<bool> AddUserRoleAsync(string userEmail, string[] roles);
    }
}