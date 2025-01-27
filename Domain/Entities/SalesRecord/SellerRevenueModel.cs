using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.SalesRecord
{
    public class SellerRevenueModel
    {
        public string Id { get; set; }
        public string PidX { get; set; }
        public double TotalAmountPaid { get; set; }
    }
}
