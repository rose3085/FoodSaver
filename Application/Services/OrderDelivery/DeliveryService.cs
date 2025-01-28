using Application.DTO.OrderDelivery;
using Application.Interfaces.Data;
using Application.Interfaces.OrderDelivery;
using Application.Response.Food;
using Domain.Entities.Foods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderDelivery
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork _uow;

        public DeliveryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<FoodServiceResponse> UpdateDeliveryStatus(UpdateDeliveryStatusDto updateDeliveryStatusDto)
        {
            try {

                var order = await _uow.AsyncRepositories<OrderModel>().GetById(updateDeliveryStatusDto.OrderId);
                if (order != null)
                {

                    if (updateDeliveryStatusDto.IsDelivered == true && order.IsDelivered == true)
                    {
                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "Order already delivered.",
                        };
                    }
                    else if (updateDeliveryStatusDto.IsDelivered == false && order.IsDelivered == false)
                    {
                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "Please select a different option",
                        };

                    }
                    else 
                    {
                       
                         order.IsDelivered = updateDeliveryStatusDto.IsDelivered;
                        await _uow.AsyncRepositories<OrderModel>().UpdateAsync(order);
                        _uow.save();

                        if (updateDeliveryStatusDto.IsDelivered == true)
                        {
                            return new FoodServiceResponse()
                            {
                                IsSuccess = true,
                                Message = $"Order delivery status updated to Delivered.",
                            };
                        }
                        else {
                            return new FoodServiceResponse()
                            {
                                IsSuccess = true,
                                Message = $"Order delivery status updated to NotDelivered.",
                            };
                        }
                       
                    }
                
                }
                else {
                    return new FoodServiceResponse()
                    {
                        IsSuccess = false,
                        Message ="Order doesn't exists",
                    };
                }

                
            }
            catch (Exception ex)
            {
                return new FoodServiceResponse()
                { IsSuccess = false,
                    Error = ex.Message
                };

            }
        }
    }
}
