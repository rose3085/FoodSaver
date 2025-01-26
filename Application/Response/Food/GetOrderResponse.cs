using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Food
{
   public class GetOrderResponse
    {

        public string Id { get; set; }
        //public string SellerName { get; set; }
        public string FoodName { get; set; }

        public bool IsDelivered { get; set; }
        public string CityName { get; set; }
    }
}
