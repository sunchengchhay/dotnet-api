using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Dtos.Role;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepo;

        public RoleController(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpGet("GetRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            var list = await _roleRepo.GetRolesAsync();
            return Ok(list);
        }


        [HttpGet("GetUserRole")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetUserRole(string userEmail)
        {
            var userClaims = await _roleRepo.GetUserRolesAsync(userEmail);
            return Ok(userClaims);
        }

        [HttpPost("AddRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(string[] role)
        {
            var userRole = await _roleRepo.AddRolesAsync(role);

            if (userRole.Count == 0)
                return BadRequest();

            return Ok(userRole);
        }

        [HttpPost("AddUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole([FromBody] PostUserRoleDto postUserRole)
        {
            var result = await _roleRepo.AddUserRoleAsync(postUserRole.Email, postUserRole.Roles);

            if (!result)
            {
                return BadRequest();
            }

            return StatusCode((int)HttpStatusCode.Created, result);
        }
    }
}