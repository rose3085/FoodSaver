using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Response.SalesRecord
{
    public class GetSalesRecordResponse
    {
        
            public string id { get; set; }
            public int totalPreviousAmount { get; set; }
            public bool dailyLimitReached { get; set; }
            public bool commissionPaid { get; set; }
            public int newAmount { get; set; }
            public Seller seller { get; set; }
        
    }

        public class Seller
        {
            public bool isDeleted { get; set; }
            public bool canPost { get; set; }
            public object foods { get; set; }
            public string id { get; set; }
            public string userName { get; set; }
            public string normalizedUserName { get; set; }
            public string email { get; set; }
            public string normalizedEmail { get; set; }
            public bool emailConfirmed { get; set; }
            public string passwordHash { get; set; }
            public string securityStamp { get; set; }
            public string concurrencyStamp { get; set; }
            public object phoneNumber { get; set; }
            public bool phoneNumberConfirmed { get; set; }
            public bool twoFactorEnabled { get; set; }
            public object lockoutEnd { get; set; }
            public bool lockoutEnabled { get; set; }
            public int accessFailedCount { get; set; }
        }

   
}
