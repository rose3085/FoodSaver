using Application.DTO.Foods;
using Application.Response.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Food
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(CreateOrderDto createOrderRequest);
    }
}
