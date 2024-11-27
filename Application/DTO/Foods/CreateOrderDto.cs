using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Foods
{
    public class CreateOrderDto
    {
       
        public string ProductId { get; set; }
        public string BuyerName { get; set; }
        public string PidX { get; set; }
        public double Amount { get; set; }


    }
}
