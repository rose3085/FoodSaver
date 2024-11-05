using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Food
{
   public class GetProductResponse
    {
        public string Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public double PricePerKg { get; set; }
        public double Quantity { get; set; }
        public bool IsBooked { get; set; }  
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
    }
}
