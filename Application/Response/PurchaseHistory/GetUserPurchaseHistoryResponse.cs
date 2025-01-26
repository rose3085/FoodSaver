using Domain.Entities.Foods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.PurchaseHistory
{
    public class GetUserPurchaseHistoryResponse
    {
        public string OrderId { get; set; }
        public FoodModel Food { get; set; }

    }
}
