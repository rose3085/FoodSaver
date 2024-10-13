using Application.DTO.User;
using Application.Interfaces.User;
using Application.Services.User;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleDto role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRole(role);
                return Ok(result);

            }
            return BadRequest();
        
        }

        [HttpPost]
        [Route("DeleteRole")]

        public async Task<IActionResult> DeleteRoleAsync([FromBody] RoleDto role)
        {


            if (ModelState.IsValid)
            {
                var result = await _roleService.DeleteRole(role);
                return Ok(result);

            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateRole")]

        public async Task<IActionResult> UpdateRoleAsync( string newName, RoleDto role)
        {


            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRole(newName,role);   
                return Ok(result);

            }
            return BadRequest();
        }

    }
}
