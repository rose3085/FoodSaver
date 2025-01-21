using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Response.PurchaseHistory
{
    public class GetPurchaseHistoryResponse
    {
        public string Id { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public double PricePerKg { get; set; }
        public double Quantity { get; set; }

        public string ProductImage { get; set; }

        public bool IsBooked { get; set; } = false;
        public DateTime Date { get; set; }

        
    }
}
