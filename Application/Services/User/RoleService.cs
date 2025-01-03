using Application.DTO.User;
using Application.Interfaces.User;
using Application.Response;
using AutoMapper;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<RoleManagerResponse> CreateRole(RoleDto role)
        { 
            
           
            try
            { 
                if (role == null)
            {
                throw new NullReferenceException("Role is null here");
            }
           
                var checkRole= await _roleManager.FindByNameAsync(role.Name);
              


                if (checkRole != null)
                {
                    return new RoleManagerResponse
                    {
                        Message = "Role Already Exists!",
                        IsSuccess = false

                    };

                }
                 var  roleModel = _mapper.Map<Role>(role);
                
                    roleModel.Id = Guid.NewGuid().ToString();
                    var result = await _roleManager.CreateAsync(roleModel);

                if (result.Succeeded)
                {
                    return new RoleManagerResponse
                    {

                        Message = "Role Created Successfully!",
                        IsSuccess = true
                    };

                }
                else
                {
                    return new RoleManagerResponse
                    {
                        Message = "Role Registration Unsuccessfull !",
                        IsSuccess = false,
                        //Error = result.Errors.ToString(),
                    };

                }
                
                
            }
            catch (Exception ex)
            {
                return new RoleManagerResponse
                {
                    Error = ex.Message,
                    IsSuccess = false,
                    Message = "Couldn't create role!!",
                };


            }
        }

        public async Task<RoleManagerResponse> DeleteRole(RoleDto role)
        {
            try
            {
                var checkRole = await _roleManager.FindByNameAsync(role.Name);
                if (checkRole == null)
                {
                    return new RoleManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Role doesn't exist",

                    };

                }
                var roleModel = _mapper.Map<Role>(checkRole);
              
                    var result = await _roleManager.DeleteAsync(roleModel);
                    if (result.Succeeded)
                    {
                        return new RoleManagerResponse
                        {
                            IsSuccess = true,
                            Message = "Role deleted successfully.",
                        };
                    }
                    else
                    {
                        return new RoleManagerResponse
                        {

                            IsSuccess = false,
                            Message=$"Couldn't delete the role {roleModel.Name}",
                        };
                    }
               

            }
            catch (Exception ex)
            {
                return new RoleManagerResponse
                { 
                 IsSuccess = false,
                 Message ="Role deletion unsuccessful!",
                 Error= ex.Message,
                };
            }
        }

        public async Task<IList<string>> GetUserRole()
        {
            try
            {
               
                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {
                    //var applicationUser = await _userManager.FindByNameAsync(user);
                    var roles = await _userManager.GetRolesAsync(userInfo);
                    if (roles.Count > 0)
                    {
                        return roles;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RoleManagerResponse> UpdateRole(string newName,RoleDto role)
        {
            try
            {
                var checkRole = await _roleManager.FindByNameAsync(role.Name);
                if (checkRole == null)
                {
                    return new RoleManagerResponse
                    { 
                        IsSuccess = false,
                        Message = "Role doesn't exist",
                    
                    };
                }
                checkRole.Name = newName;
                var roleModel = _mapper.Map<Role>(checkRole);
                var result =await _roleManager.UpdateAsync(roleModel);
                if (result.Succeeded)
                {
                    return new RoleManagerResponse
                    {
                        IsSuccess = true,
                        Message = "Role updated successfully.",
                    };
                }
                else
                {
                    return new RoleManagerResponse
                    {

                        IsSuccess = false,
                        Message = $"Couldn't update the role {roleModel.Name}",
                    };
                }

            }
            catch (Exception ex) {

                return new RoleManagerResponse
                {
                    IsSuccess = false,
                    Message = "Couldn't update the role",
                    Error = ex.Message,

                };
            }
        }
    }
}
