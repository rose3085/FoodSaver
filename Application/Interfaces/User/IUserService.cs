using Application.DTO.User;
using Application.Response;
using Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.User
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUser(UserRegisterRequest registerRequest, string role);
        Task<UserLoginResponse> LoginUser(UserLoginRequest loginRequest);
        Task<UserManagerResponse> LogoutUser(UserLoginRequest logoutRequest);
        Task<UserManagerResponse> DeleteUser(UserLoginRequest deleteRequest);
        Task<UserManagerResponse> UpdatePassword(UpdatePasswordDto updatePasswordRequest);
        Task<UserManagerResponse> UpdateEmail(UpdateEmailDto updateEmailRequest);

        Task<ApplicationUser> GetUserByName(string name);

        Task<UserManagerResponse> UpdateUser(UpdateUser user);

        //Task<IEnumerable<ApplicationUser>> GetAllUser();
    }
}
