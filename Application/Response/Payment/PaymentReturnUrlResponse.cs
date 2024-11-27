using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Response.Payment
{
    public class PaymentReturnUrlResponse
    {
        public string Pidx { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string purchase_order_id { get; set; }
        public string purchase_order_name { get; set; }
    }
}
