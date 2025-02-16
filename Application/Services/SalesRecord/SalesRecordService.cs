using Application.DTO.SalesRecord;
using Application.Interfaces.Data;
using Application.Interfaces.SalesRecord;
using Application.Response.Food;
using Azure.Core;
using Domain.Entities.Foods;
using Domain.Entities.SalesRecord;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.SalesRecord
{
    public class SalesRecordService : ISalesRecordService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesRecordService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<SalesRecordModel>> GetAllRecord()
        {
            var result = await _uow.AsyncRepositories<SalesRecordModel>().GetAllAsync();
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return result;
                }
                else { return null; }
            }
            else
            { return null; }
        }

        public async Task<SalesRecordModel> GetSingleRecord(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var sellerId = user?.Id;
            // var result = await _uow.AsyncRepositories<SalesRecordModel>().GetById(sellerId);
            var checkNewSellerId = await GetAllRecord();
            if (checkNewSellerId != null)
            { 
                foreach (var idExists in checkNewSellerId)

                {
                    if (idExists.Seller.Id == sellerId)
                    {
                        return idExists;
                    }

                    else
                    { return null; }
                }
        }
            else
            { return null; }
            return null;
        }


        public async Task<FoodServiceResponse> AddNewRecord(PostSalesRecordDto request)
        {
            var id = Guid.NewGuid().ToString();
            var requestModel = new SalesRecordModel()
            {
                Id = id,
                TotalPreviousAmount = 0,
                NewAmount = request.NewAmount,
                Seller = request.SellerId,
                DailyLimitReached = false,
                CommissionPaid = true,

            };
            var result = await _uow.AsyncRepositories<SalesRecordModel>().AddAsync(requestModel);
            _uow.save();
            if (request != null)
            {
                return new FoodServiceResponse()
                {
                    Message = "Sales record Updated sucessfully!",
                    IsSuccess = true,
                };

            }
            else 
            {
                return new FoodServiceResponse()
                {
                    Message = "Sales record couldn't be Updated!",
                    IsSuccess = false,
                };
            }
        }


        public async Task<FoodServiceResponse> PostAmountUpdate(PostSalesRecordDto request)
        {
            try {
                var checkNewSellerId = await GetAllRecord();
               //if(checkNewSellerId == null)
               // {await AddNewRecord(request);
               //     return new FoodServiceResponse()
               //     {
               //         Message = "Sales record Updated sucessfully!",
               //         IsSuccess = true,
               //     };
               // }

                    if (checkNewSellerId.Count() == 0 || checkNewSellerId == null)
                    {
                        AddNewRecord(request);
                    }
                    else
                    {
                        foreach (var idExists in checkNewSellerId)

                        {
                            if (idExists.Seller != request.SellerId)
                            {
                                var id = Guid.NewGuid().ToString();
                                var requestModel = new SalesRecordModel()
                                {
                                    Id = id,
                                    TotalPreviousAmount = 0,
                                    NewAmount = request.NewAmount,
                                    Seller = request.SellerId,
                                    DailyLimitReached = false,
                                    CommissionPaid = true,

                                };
                                var result = await _uow.AsyncRepositories<SalesRecordModel>().AddAsync(requestModel);
                                _uow.save();
                                if (request != null)
                                {
                                    return new FoodServiceResponse()
                                    {
                                        Message = "Sales record Updated sucessfully!",
                                        IsSuccess = true,
                                    };

                                }

                            }
                            else
                            {
                                idExists.NewAmount += request.NewAmount;
                                if (idExists.NewAmount >= 200)
                                {
                                    idExists.DailyLimitReached = true;
                                    idExists.CommissionPaid = false;
                                    var getSeller = await _userManager.FindByIdAsync(request.SellerId.Id);
                                    getSeller.CanPost = false;
                                    await _userManager.UpdateAsync(getSeller);

                                }

                                await _uow.AsyncRepositories<SalesRecordModel>().UpdateAsync(idExists);

                                _uow.save();
                                return new FoodServiceResponse()
                                {
                                    Message = "Sales record Updated sucessfully!",
                                    IsSuccess = true,
                                };

                            }
                        }

                    }
                    return new FoodServiceResponse()
                    {
                        Message = "Sales record  Updated!",
                        IsSuccess = true,
                    };
                
                //else
                //{
                //    return new FoodServiceResponse()
                //    {
                //        Message = "Sales record coulldn't be  Updated!",
                //        IsSuccess = false,
                //    };
                //}

            }
            catch (Exception ex)
            {
                return new FoodServiceResponse() { 
                    IsSuccess = false,
                    Error = ex.Message
                };
            
            }
        }


        public async Task<bool> AddNewSellerRevenue(PostSellerRevenueDto request,SalesRecordModel sellerSalesRecord)
        {
            try {
                if (request != null && sellerSalesRecord != null)
                {
                    var id = Guid.NewGuid().ToString();

                    var revenueModel = new SellerRevenueModel()
                    { 
                        Id = id,
                        PidX = request.PidX,
                        TotalAmountPaid = request.Amount
                    };

                    sellerSalesRecord.RevenueModel = revenueModel;
                    if (request.Amount >= 20.0)
                    {
                        sellerSalesRecord.CommissionPaid = true;
                     
                    }
                    await _uow.AsyncRepositories<SalesRecordModel>().UpdateAsync(sellerSalesRecord);
                    _uow.save();
                    return true;
                }
                else { return false; }
            
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> UpdateSellerRevenue(PostSellerRevenueDto request, SalesRecordModel sellerSalesRecord)
        {
            try
            {
                if (request != null && sellerSalesRecord != null)
                {
                    var oldAmount = sellerSalesRecord.RevenueModel.TotalAmountPaid;
                    var newAmount = request.Amount + oldAmount;
                    sellerSalesRecord.RevenueModel.PidX = request.PidX;
                    sellerSalesRecord.RevenueModel.TotalAmountPaid = newAmount;
                    
                    if (request.Amount >= 20.0)
                    {
                        sellerSalesRecord.CommissionPaid = true;
                        sellerSalesRecord.TotalPreviousAmount += sellerSalesRecord.NewAmount;
                        sellerSalesRecord.NewAmount = 0;
                        sellerSalesRecord.DailyLimitReached = false;
                        sellerSalesRecord.Seller.CanPost = true;
                    }
                    await _uow.AsyncRepositories<SalesRecordModel>().UpdateAsync(sellerSalesRecord);
                    _uow.save();
                    return true;
                }
                else { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public async Task<FoodServiceResponse> PostSellerRevenueUpdate(PostSellerRevenueDto request)
        {
            try {


                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {

                    var includes = new Expression<Func<SalesRecordModel, object>>[]
                      {
                           s => s.RevenueModel,
                           s => s.Seller
                      };
                    Expression<Func<SalesRecordModel, bool>> filter = x => x.Seller == userInfo;
                    var salesRecord = await _uow.AsyncRepositories<SalesRecordModel>().GetwithIncludeAndFilter(includes, filter);

                    if (salesRecord != null)
                    {
                        if (salesRecord.Count() > 0)
                        { 
                            var sellerSalesRecord = salesRecord.FirstOrDefault();
                            if (sellerSalesRecord?.RevenueModel == null)
                            {
                               var result = await AddNewSellerRevenue(request,sellerSalesRecord);
                                if (result == true)
                                {
                                    return new FoodServiceResponse()
                                    {
                                        IsSuccess = true,
                                        Message = "Seller revenue updated sucessfullt!"
                                    };
                                }
                                else 
                                {
                                    return new FoodServiceResponse()
                                    {
                                        IsSuccess = false,
                                        Message = "Couldn't update seller revenue."
                                    };
                                }
                               
                            }
                            else 
                            {
                                var result = await UpdateSellerRevenue(request, sellerSalesRecord);
                                if (result == true)
                                {
                                    return new FoodServiceResponse()
                                    {
                                        IsSuccess = true,
                                        Message = "Seller revenue updated sucessfullt!"
                                    };
                                }
                                else
                                {
                                    return new FoodServiceResponse()
                                    {
                                        IsSuccess = false,
                                        Message = "Couldn't update seller revenue."
                                    };
                                }

                            }

                        }
                        else
                        {

                            return new FoodServiceResponse()
                            {
                                IsSuccess = false,
                                Message = "Couldn't update seller revenue."
                            };
                        }


                    }
                    else
                    {

                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "Couldn't update seller revenue."
                        };
                    }

                }
                else {

                    return new FoodServiceResponse()
                    {
                        IsSuccess = false,
                        Message = "Couldn't update seller revenue."
                    };
                }

                    

                
            
            
            }
            catch(Exception ex)
            {
                return new FoodServiceResponse()
                {
                    IsSuccess = false,
                    Error = ex.Message
                };

            }
        }
    }
}
