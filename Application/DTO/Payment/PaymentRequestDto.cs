using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Payment
{
    public class PaymentRequestDto
    {
        public string PidX { get; set; }
        public double Amount { get; set; }
    }
}
