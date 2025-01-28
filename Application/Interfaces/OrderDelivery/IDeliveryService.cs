using Application.DTO.OrderDelivery;
using Application.Response.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.OrderDelivery
{
    public interface IDeliveryService
    {
        Task<FoodServiceResponse> UpdateDeliveryStatus(UpdateDeliveryStatusDto updateDeliveryStatusDto);
    }
}
