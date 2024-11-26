using Domain.Entities.Payment;
using Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Foods
{
    public class OrderModel
    {
        public string Id { get; set; }

        public ApplicationUser Buyer { get; set; }

        public FoodModel Food { get; set; }
        public PaymentModel Payment { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
