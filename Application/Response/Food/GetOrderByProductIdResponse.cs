using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Food
{
    public class GetOrderByProductIdResponse
    {

        public string BuyerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {  get; set; }
        public string FoodName { get; set; }
        public bool IsDelivered { get; set; }
        //public bool IsPaid { get; set; }
        public string CityName { get; set; }
        public string WardNumber { get; set; }
        public string ToleName { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
