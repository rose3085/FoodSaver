using Application.DTO.SalesRecord;
using Application.Response.Food;
using Domain.Entities.SalesRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.SalesRecord
{
    public interface ISalesRecordService
    {
        Task<FoodServiceResponse> PostAmountUpdate(PostSalesRecordDto request);
        Task<IEnumerable<SalesRecordModel>> GetAllRecord();
        Task<SalesRecordModel> GetSingleRecord(string sellerId);
    }
}
