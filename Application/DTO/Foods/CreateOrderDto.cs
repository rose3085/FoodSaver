using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Foods
{
    public class CreateOrderDto
    {
        public string BuyerId {  get; set; }
        public string ProductId { get; set; }
        public double Quantity { get; set; }

    }
}
