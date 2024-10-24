using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Model
{
    public class UpdateEmailRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewEmail { get; set; }
    }
}
