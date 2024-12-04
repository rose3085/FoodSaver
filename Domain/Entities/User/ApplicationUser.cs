using Domain.Entities.Foods;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class ApplicationUser : IdentityUser
    {
        // using Microsoft.AspNet.Identity; doesn't allow Generic identityUser 

        public bool IsDeleted { get; set; } = false;
        public bool CanPost { get; set; } = true;
        public ICollection<FoodModel> Foods { get; set; }

    }
}
