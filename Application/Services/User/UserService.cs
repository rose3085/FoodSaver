using Application.DTO.User;
using Application.Interfaces.User;
using Application.Response;
using AutoMapper;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User
{
    public class UserService : IUserService
    {


        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager ,
                            IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserManagerResponse> RegisterUser(UserRegisterRequest registerRequest, string role)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (userExists != null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't create user!!",
                    };
                }

                var registerModel = _mapper.Map<ApplicationUser>(registerRequest);
                var roleExists = await _roleManager.FindByNameAsync(role);
                if (roleExists != null)
                {

                    registerModel.Id = Guid.NewGuid().ToString();

                    var result = await _userManager.CreateAsync(registerModel, registerRequest.Password);
                    var addUserRole = await _userManager.AddToRoleAsync(registerModel, role);


                    if (!addUserRole.Succeeded)
                    {
                        //await _userManager.DeleteAsync(registerModel);
                        return new UserManagerResponse
                        {
                            IsSuccess = true,
                            Message = "User created successfully.",
                            Error = "Couldn't assign role to the user"
                        };
                    }

                    if (result.Succeeded && addUserRole.Succeeded)
                    {

                        return new UserManagerResponse
                        {
                            IsSuccess = true,
                            Message = "User created successfully.",
                            Role = $"Assigned role: {role}"
                        };
                    }
                    else
                    {
                        return new UserManagerResponse
                        {
                            IsSuccess = false,
                            Message = "Couldn't create user!!",
                        };

                    }
                }
                else 
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't create user!!",
                        Error ="Role doesn't exist!"
                    };
                }

            }
            catch(Exception ex)
            {
              
                return new UserManagerResponse 
             { 
                 IsSuccess = false,
                 Error = ex.Message };
            
            }


           
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
