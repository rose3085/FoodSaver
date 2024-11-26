using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.KhaltiServices
{
    public interface IKhaltiService
    {
        Task<string> KhaltiLaunch();
    }
}
