using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Model
{
    public class PostFoodRequest
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public double PricePerKg { get; set; }
        public double Quantity { get; set; }
        public string WardNumber { get; set; }
        public string ToleName { get; set; }
        public string CityName { get; set; }

        public byte[] ImageFile { get; set; }
    }
}
