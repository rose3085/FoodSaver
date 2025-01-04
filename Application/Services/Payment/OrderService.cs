using Application.DTO.Foods;
using Application.DTO.Payment;
using Application.DTO.SalesRecord;
using Application.Interfaces.Data;
using Application.Interfaces.Payment;
using Application.Interfaces.SalesRecord;
using Application.Response.Food;
using Application.Services.SalesRecord;
using AutoMapper;
using Domain.Entities.Foods;
using Domain.Entities.Location;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Payment
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISalesRecordService _salesRecordService;

        public OrderService(IMapper mapper, UserManager<ApplicationUser> userManager, IUnitOfWork uow,
             IHttpContextAccessor httpContextAccessor, IPaymentService paymentService, ISalesRecordService salesRecordService)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _paymentService = paymentService;
            _httpContextAccessor = httpContextAccessor;
            _salesRecordService = salesRecordService;
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
            try
            {

                var userInfo = await _userManager.FindByNameAsync(createOrderRequest.BuyerName);
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

                    var includes = new Expression<Func<FoodModel, object>>[]
                   {
                        s => s.Seller,
                       
                   };
                    var checkProductExist = await _uow.AsyncRepositories<FoodModel>().GetWithIncludeAndId(createOrderRequest.ProductId, includes);
                    if (checkProductExist == null)
                    {
                        return new OrderResponse
                        {
                            IsSuccess = false,
                            Message = "Please select a valid product!"
                        };
                    }
                    var paymentRequest = new PaymentRequestDto
                    {
                        Amount = checkProductExist.PricePerKg,
                        PidX = createOrderRequest.ProductId,
                    };
                    var createPayment = await _paymentService.AddPaymentInfoAsync(paymentRequest);
                    if (createPayment != null)
                    {


                        var id = Guid.NewGuid().ToString();
                        var deliveryId = Guid.NewGuid().ToString();

                        var deliveryAddress = new DeliveryAddressModel
                        { 
                            Id = deliveryId,
                            WardNumber = createOrderRequest.WardNumber,
                            ToleName = createOrderRequest.ToleName,
                            CityName = createOrderRequest.CityName,
                        
                        };
                        var order = new OrderModel
                        {
                            Id = id,
                            Buyer = userInfo,
                            Food = checkProductExist,
                            Payment = createPayment,
                            CreatedTime = DateTime.UtcNow,
                            DeliveryAddress = deliveryAddress,
                        };

                        checkProductExist.IsBooked = true;

                        //var request = _mapper.Map<OrderModel>(order);
                        var result = await _uow.AsyncRepositories<OrderModel>().AddAsync(order);
                        await _uow.AsyncRepositories<FoodModel>().UpdateAsync(checkProductExist);

                        var sellerId = checkProductExist.Seller;
                        if (sellerId != null)
                        {
                            var updateSaleRecord = new PostSalesRecordDto()
                            {
                                SellerId = sellerId,
                                NewAmount = checkProductExist.PricePerKg,

                            };
                            var request = await _salesRecordService.PostAmountUpdate(updateSaleRecord);
                          
                        }

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
                            Message = "Payment Unsuccessful! "
                        };
                    }
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
            catch (Exception ex)
            {

                return new OrderResponse
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }
    }
}
