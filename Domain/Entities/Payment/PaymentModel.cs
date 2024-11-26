using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Payment
{
    public class PaymentModel
    {
        public string Id { get; set; }
        public string PidX { get; set; }
        public double Amount { get; set; }
    }
}
