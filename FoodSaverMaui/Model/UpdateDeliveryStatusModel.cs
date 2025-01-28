using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Model
{
    public class UpdateDeliveryStatusModel
    {
        public string OrderId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
