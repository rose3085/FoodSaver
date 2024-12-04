using Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.SalesRecord
{
    public class PostSalesRecordDto
    {
        public double TotalPreviousAmount { get; set; }
        public bool DailyLimitReached { get; set; }
        public bool CommissionPaid { get; set; }
        public double NewAmount { get; set; }
        public ApplicationUser SellerId { get; set; }
    }
}
