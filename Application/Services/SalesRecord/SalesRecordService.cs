using Application.DTO.SalesRecord;
using Application.Interfaces.Data;
using Application.Interfaces.SalesRecord;
using Application.Response.Food;
using Domain.Entities.SalesRecord;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.SalesRecord
{
    public class SalesRecordService : ISalesRecordService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesRecordService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }
        public async Task<IEnumerable<SalesRecordModel>> GetAllRecord()
        {
            var result = await _uow.AsyncRepositories<SalesRecordModel>().GetAllAsync();
            if (result != null)
            { return result; }
            else
            { return null; }
        }

        public async Task<SalesRecordModel> GetSingleRecord(string sellerId)
        {
            var result = await _uow.AsyncRepositories<SalesRecordModel>().GetById(sellerId);
            if (result != null)
            { return result; }
            else
            { return null; }
        }

        public async Task<FoodServiceResponse> PostAmountUpdate(PostSalesRecordDto request)
        {
            try {
                var checkNewSellerId = await GetAllRecord();
                foreach (var idExists in checkNewSellerId)

                {
                    if (idExists.Seller == request.SellerId)
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
                return new FoodServiceResponse()
                {
                    Message = "Sales record couldn't be Updated!",
                    IsSuccess = false,
                };

            }
            catch (Exception ex)
            {
                return new FoodServiceResponse() { 
                    IsSuccess = false,
                    Error = ex.Message
                };
            
            }
        }
    }
}
