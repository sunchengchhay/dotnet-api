using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Role
{
    public class PostUserRoleDto
    {
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}