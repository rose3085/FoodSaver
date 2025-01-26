using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hubs.Models
{
    public class NotificationDto
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public string Message { get; set; }
    }
}
