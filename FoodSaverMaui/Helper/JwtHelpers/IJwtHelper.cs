using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Helper
{
    public interface IJwtHelper
    {
        string ExtractUserInfo(string jwtToken);
    }
}
