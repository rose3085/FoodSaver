using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Model
{
    public class KhaltiPayload
    {
        public string pidx {  get; set; }
        public long totalAmount { get; set; }
        public string status { get; set; }
        public string transactionId { get; set; }
        public string fee { get; set; }

        
    //    PaymentPayload {
    //pidx: String,
    //totalAmount: Long,
    //status: String,
    //transactionId: String,
    //fee: Long,
    //refunded: Boolean
    //purchaseOrderId: String,
    //purchaseOrderName: String,
    //extraMerchantParams: Map<String, Object>
    //}
}
}
