using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Utilities;
using BusinessLayer.Models.RequestModel.OjtBatchRequest;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;

namespace API.Controllers.OJTBatchController
{
    [Route("api/ojtbatch")]
    [ApiController]
    public class OJTBatchController : ControllerBase
    {
        private readonly IOJTBatchService _ojtService;
        private readonly IHubContext<SignalHub> _hubContext;
        public OJTBatchController(IOJTBatchService ojtService, IHubContext<SignalHub> hubContext)
        {
            _ojtService = ojtService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpGet]
        [Route("ongoing-batches")]
        public async Task<IActionResult> GetValidOJTBatchInformation()
        {
            try
            {
                var list = await _ojtService.GetValidOJtList();
                return Ok(list);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}")]        
        public async Task<IActionResult> GetDetailOJTBatchInformation(int id)
        {
            try
            {
                var list = await _ojtService.GetDetailOjtBatch(id);
                return Ok(list);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOJTBatchInformation([FromBody]CreateOjtBatchRequest request)
        {
            try
            {
                await _ojtService.CreateOjtBatch(request);              
                return StatusCode(StatusCodes.Status201Created, "Batch is created successfully");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("batches-of-university")]
        public async Task<IActionResult> GetValidOJTBatchInformation(int universityid)
        {
            try
            {
                var list = await _ojtService.GetValidOJtListbyUniversityId(universityid);
                return Ok(list);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }

    }
}
