using Application.Interfaces.Data;
using Application.Interfaces.PurchaseHistory;
using Application.Response.Food;
using Application.Response.PurchaseHistory;
using Domain.Entities.Foods;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PurchaseHistory
{
    public class PurchaseHistoryService : IPurchaseHistoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseHistoryService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<GetUserPurchaseHistoryResponse>> GetUserPurchasedFoods()
        {
            try
            {
                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {
                    var includes = new Expression<Func<OrderModel, object>>[]
                      {
                            s => s.Food,
                            s => s.Buyer,

                      };

                    Expression<Func<OrderModel, bool>> filter = x => x.Buyer == userInfo;
                    var foods = await _uow.AsyncRepositories<OrderModel>().GetwithIncludeAndFilter(includes,filter);
                    if (foods.Count() > 0)
                    {
                        var foodResult = foods
                                    .Select(async food =>
                                    {

                                        return new GetUserPurchaseHistoryResponse()
                                        { Food = food.Food };
                                    }).ToList();
                        return await Task.WhenAll(foodResult);
                    }
                    else
                    { return null; }

                }
                else 
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
