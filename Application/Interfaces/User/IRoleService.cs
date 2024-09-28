using Application.DTO.User;
using Application.Response;
using Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.User
{
    public interface IRoleService
    {
        Task<RoleManagerResponse> CreateRole(RoleDto role);
        Task<RoleManagerResponse> DeleteRole(RoleDto role);
        Task<RoleManagerResponse> UpdateRole(string newName, RoleDto role);

    }
}
