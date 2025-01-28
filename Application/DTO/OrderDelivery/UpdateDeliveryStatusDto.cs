using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.OrderDelivery
{
    public class UpdateDeliveryStatusDto
    {
        public string OrderId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
