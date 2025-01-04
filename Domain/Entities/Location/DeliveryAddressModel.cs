using Domain.Entities.Foods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Location
{
    public class DeliveryAddressModel
    {
        public string Id { get; set; }
        public string WardNumber { get; set; }

        public string ToleName { get; set; }

        public string CityName { get; set; }

        public string OrderId { get; set; }
        public OrderModel Order { get; set; }
    }
}
