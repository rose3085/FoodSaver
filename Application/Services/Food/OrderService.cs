using Application.DTO.Foods;
using Application.Interfaces.Data;
using Application.Interfaces.Food;
using Application.Response.Food;
using AutoMapper;
using Domain.Entities.Foods;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Food
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IMapper mapper, UserManager<ApplicationUser> userManager, IUnitOfWork uow,
             IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

    //    try { }
    //        catch (Exception ex) {

    //            return new OrderResponse
    //            {
    //                IsSuccess = false,
    //                Error = ex.Message
    //};

        public async Task<OrderResponse> CreateOrder(CreateOrderDto createOrderRequest)
        {
            try {

                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {
                    var userId = userInfo?.Id;
                    

                    var userRole = await _userManager.GetRolesAsync(userInfo);
                    if (!userRole.Contains("Buyer"))
                    {
                        return new OrderResponse
                        {
                            IsSuccess = false,
                            Message = "You must be buyer to place an order!"
                        };
                    }

                    var checkProductExist = await _uow.AsyncRepositories<FoodModel>().GetById(createOrderRequest.ProductId);
                    if (checkProductExist == null)
                    {
                        return new OrderResponse
                        {
                            IsSuccess = false,
                            Message = "Please select a valid product!"
                        };
                    }
                   
                    var id = Guid.NewGuid().ToString();
                    var order =  new OrderModel
                    {
                        Id=id,
                        Buyer = userInfo,
                        Food = checkProductExist,
                        //
                        //Quantity = createOrderRequest.Quantity,

                    };

                    checkProductExist.IsBooked = true;

                    //var request = _mapper.Map<OrderModel>(order);
                    var result = await _uow.AsyncRepositories<OrderModel>().AddAsync(order);
                    await _uow.AsyncRepositories<FoodModel>().UpdateAsync(checkProductExist);
                    _uow.save();

                    return new OrderResponse
                    {
                        IsSuccess = true,
                        Message = "Order placed Successfully"
                    };


                }
                else
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Login to your account to place an order!"
                    };
                }
            }
            catch (Exception ex) {

                return new OrderResponse
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }
    }
}
