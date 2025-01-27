using Application.DTO.SalesRecord;
using Application.Interfaces.SalesRecord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.SalesRecord
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesRecordController : ControllerBase
    {
        private readonly ISalesRecordService _salesRecordService;

        public SalesRecordController(ISalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        [HttpGet]
        [Route("GetSingleRecord")]
        public async Task<IActionResult> GetSingleRecord(string userName)
        {
            if (ModelState.IsValid)
            {
                var request = await _salesRecordService.GetSingleRecord(userName);
                return Ok(request);
            }
            else 
            {

                return BadRequest();
            }
        
        }
        [HttpGet]
        [Route("GetAllRecord")]
        public async Task<IActionResult> GetAllRecord()
        {
            if (ModelState.IsValid)
            {
                var request = await _salesRecordService.GetAllRecord();
                return Ok(request);
            }
            else
            {

                return BadRequest();
            }

        }



        [HttpPost]
        [Route("SalesRecorsUpdate")]
        public async Task<IActionResult> PostSalesRecord(PostSalesRecordDto requestModel)
        {
            if (ModelState.IsValid)
            {
                var request = await _salesRecordService.PostAmountUpdate(requestModel);
                return Ok(request);
            }
            else
            {

                return BadRequest();
            }


        }




        [HttpPost]
        [Route("SellerRevenueUpdate")]
        public async Task<IActionResult> PostSellerRevenue(PostSellerRevenueDto postSellerRevenueDto)
        {
            if (ModelState.IsValid)
            {
                var request = await _salesRecordService.PostSellerRevenueUpdate(postSellerRevenueDto);
                return Ok(request);
            }
            else
            {

                return BadRequest();
            }


        }
    }
}
