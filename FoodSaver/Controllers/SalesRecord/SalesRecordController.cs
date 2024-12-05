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
    }
}
