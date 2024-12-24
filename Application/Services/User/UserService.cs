using Application.DTO.User;
using Application.Interfaces.Data;
using Application.Interfaces.User;
using Application.Response;
using AutoMapper;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager , SignInManager<ApplicationUser> signInManager,
                            IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
                            IUnitOfWork uow, IMapper mapper,IRoleService roleService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _uow = uow;
            _roleService = roleService;
        }

        public async Task<UserManagerResponse> DeleteUser(UserLoginRequest deleteRequest)
        {
            try {
                //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                var userExists = await _userManager.FindByEmailAsync(deleteRequest.Email);
                if (userExists == null || userInfo != userExists)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };

                }
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExists, deleteRequest.Password);
                if (!isPasswordCorrect)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };
                }
                await _signInManager.SignOutAsync();
                //var result = await _userManager.DeleteAsync(userInfo);
                userInfo.IsDeleted = true;
                var result = await _userManager.UpdateAsync(userInfo);

                if (result == null)
                {

                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't delete user!!",
                       
                    };

                }
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "User Deleted Sucessfully",
                    
                };
            }

            catch(Exception ex) {
               return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Couldn't delete user!!",
                    Error = ex.Message
                };
            }
        }

    

        public async Task<ApplicationUser> GetUserByName(string name)
        {
            try { 
                    var result = await _userManager.FindByNameAsync(name);
                if (result != null)
                {
                    return result;
                }
                else 
                {
                    return null;
                }
            
            }
            catch (Exception ex) { return null; }
        }



        public async Task<UserLoginResponse> LoginUser(UserLoginRequest loginRequest)
        {
            try
            {
                //var userExists = await _userManager.FindByEmailAsync(loginRequest.Email);
                var userExists = await _userManager.FindByNameAsync(loginRequest.UserName);
                if (userExists == null || userExists.IsDeleted == true && userExists.Email != loginRequest.Email)
                {
                    return new UserLoginResponse
                    { 
                        IsSuccess = false,
                        Message= "Invalid Credentials!!"
                    };

                }


                var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExists, loginRequest.Password);
                if (!isPasswordCorrect)
                {
                    return new UserLoginResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };
                }
                var getRole = await _userManager.GetRolesAsync(userExists);
               
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userExists.UserName),
                new Claim(ClaimTypes.NameIdentifier, userExists.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                
            };
                foreach (var userRole in getRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = GenerateNewJsonWebToken(authClaims);
                if (token != null)
                {
                    return new UserLoginResponse
                    {
                        IsSuccess = true,
                        Message = "User login Successful",
                        Token = token,
                        Role = getRole,
                    };

                }
                else 
                {

                    return new UserLoginResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };
                }

                


            }
            catch (Exception ex)
            {
                return new UserLoginResponse
                    
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<UserManagerResponse> LogoutUser(UserLoginRequest logoutRequest)
        {
            try
            {

                var userExists = await _userManager.FindByEmailAsync(logoutRequest.Email);
                if (userExists == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };

                }
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExists, logoutRequest.Password);
                if (!isPasswordCorrect)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!!"
                    };
                }

                var result =  _signInManager.SignOutAsync();
                if (result == null)
                {

                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't sign out user!!",

                    };

                }
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "User logout Sucessfull",

                };
            }

            catch (Exception ex)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Couldn't delete user!!",
                    Error = ex.Message
                };
            }
        }

        public async Task<UserManagerResponse> RegisterUser(UserRegisterRequest registerRequest, string role)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (userExists != null && userExists.IsDeleted != true )
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't create user!!",
                    };
                }
                if (userExists !=null && userExists.IsDeleted == true && userExists.UserName == registerRequest.UserName)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid username",
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

        public async Task<UserManagerResponse> UpdateEmail(UpdateEmailDto updateEmailRequest)
        {
            try {
                var checkEmail = await _userManager.FindByEmailAsync(updateEmailRequest.Email);
                if (checkEmail == null )
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!",
                    };
                }
                var checkPassword = await _userManager.CheckPasswordAsync(checkEmail, updateEmailRequest.Password);
                if (!checkPassword)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!",
                    };
                }

                if (updateEmailRequest.Email != updateEmailRequest.NewEmail)
                {
                    var checkNewEmail = await _userManager.FindByEmailAsync(updateEmailRequest.NewEmail);
                    if (checkNewEmail == null)
                    {
                        //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                        var token = await _userManager.GenerateChangeEmailTokenAsync(checkEmail,updateEmailRequest.NewEmail);
                        var updateRequest = await _userManager.ChangeEmailAsync(checkEmail, updateEmailRequest.NewEmail, token);
                        if (updateRequest != null)
                        {
                            return new UserManagerResponse
                            {
                                IsSuccess = true,
                                Message = "Email updated successfully.",
                            };
                        }
                        else {

                            return new UserManagerResponse
                            {
                                IsSuccess = false,
                                Message = "Couldn't update email",
                            };
                        }
                    }
                    else
                    {
                        return new UserManagerResponse
                        {
                            IsSuccess = false,
                            Message = "Please enter an unique email!",
                        };
                    }
                }
                else 
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "New email can't be same as previous email",
                    };
                }

            }
            catch (Exception ex)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<UserManagerResponse> UpdatePassword(UpdatePasswordDto updatePasswordRequest)
        {
            try { 
            
                var checkEmail = await _userManager.FindByEmailAsync(updatePasswordRequest.Email);
                if (checkEmail == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!",
                    };
                }
                
                var checkPassword = await _userManager.CheckPasswordAsync(checkEmail,updatePasswordRequest.OldPassword);
                if (!checkPassword)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Credentials!",
                    };
                }

                if (updatePasswordRequest.NewPassword == updatePasswordRequest.ConfirmNewPassword)
                {
                    var updateRequest = await _userManager.ChangePasswordAsync(checkEmail, updatePasswordRequest.OldPassword, updatePasswordRequest.NewPassword);
                    if (updateRequest != null)
                    {
                        return new UserManagerResponse
                        {
                            IsSuccess = true,
                            Message = "Password updated successfully.",
                        };
                    }
                    else
                    {

                        return new UserManagerResponse
                        {
                            IsSuccess = false,
                            Message = "Couldn't Update your password",
                        };
                    }

                }
                else {

                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Confirm Password didn't match New Password",
                    };
                }
            
            }
            catch (Exception ex)
            {

                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Error = ex.Message
                };

            }

        }

        public async Task<UserManagerResponse> UpdateUser(UpdateUser user)
        {
            //var request = await _userManager.UpdateAsync();

            try
            {

                var userInfo = await _userManager.FindByEmailAsync(user.Email);
                if (userInfo == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                       Message = "user doesn't exist!"
                    };
                }

                var checkPassword = await _userManager.CheckPasswordAsync(userInfo,user.Password);
                if (checkPassword == false)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "invalid credentials"
                    };

                }
                if (user.UserName != "" )
                {
                    userInfo.UserName = user.UserName;
                }
                if (user.PhoneNumber !="")
                {
                    userInfo.PhoneNumber = user.PhoneNumber;
                }
                if( user.Role != "")
                {
                    var userRoles = await _userManager.GetRolesAsync(userInfo);
                    if (!userRoles.Contains(user.Role))
                    {
                        var addRole = await _userManager.AddToRoleAsync(userInfo, user.Role);
                        if (addRole == null)
                        {
                            return new UserManagerResponse
                            {
                                IsSuccess = false,
                                Message = "invalid role"
                            };
                        }
                    }
                }

                var updateResult = await _userManager.UpdateAsync(userInfo);
                if (updateResult != null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = true,
                        Message = "user updated sucessfully"
                    };

                }
                else 
                
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Couldn't update user!"
                    };
                }

            
            }
            catch (Exception ex)
            {

                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
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
