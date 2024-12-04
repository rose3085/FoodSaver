using Domain.Entities.Foods;
using Domain.Entities.User;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.SalesRecord
{
    public class SalesRecordModel
    {
        public string Id { get; set; }
        public double TotalPreviousAmount { get; set; }
        public bool DailyLimitReached { get; set; }
        public bool CommissionPaid { get; set; }
        public double NewAmount { get; set; }
        public ApplicationUser Seller { get; set; }

    }
}
