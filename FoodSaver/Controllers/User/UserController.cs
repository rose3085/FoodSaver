using Application.DTO.User;
using Application.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;   
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserRegisterRequest registerRequest, string role)
        {
            if (ModelState.IsValid)
            { 
            var result = await _userService.RegisterUser(registerRequest, role);
                return Ok(result);
            
            }
            return BadRequest(ModelState);
        
        }


        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser(UserLoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(loginRequest);
                return Ok(result);

            }
            return BadRequest(ModelState);

        }
    }
}
