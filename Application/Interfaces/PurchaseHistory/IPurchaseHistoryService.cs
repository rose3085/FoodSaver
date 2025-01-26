using Application.Response.PurchaseHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.PurchaseHistory
{
    public interface IPurchaseHistoryService
    {
        Task<IEnumerable<GetUserPurchaseHistoryResponse>> GetUserPurchasedFoods();
    }
}
