using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.User
{
    public  class RoleDto
    {
        [Required]
        public string? Name { get; set; } 
    }
}
