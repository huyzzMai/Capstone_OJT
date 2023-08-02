using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.OJTBatchController
{
    [Route("api/ojtbatch")]
    [ApiController]
    public class OJTBatchController : ControllerBase
    {
        private readonly IOJTBatchService _ojtService;

        public OJTBatchController(IOJTBatchService ojtService)
        {
            _ojtService = ojtService;
        }

        [Authorize]
        [HttpGet]
        [Route("active-batch")]
        public async Task<IActionResult> GetValidOJTBatchInformation()
        {
            try
            {
                var list = await _ojtService.GetValidOJtList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("batches-of-university")]
        public async Task<IActionResult> GetValidOJTBatchInformation(int id)
        {
            try
            {
                var list = await _ojtService.GetValidOJtListbyUniversityId(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

    }
}
