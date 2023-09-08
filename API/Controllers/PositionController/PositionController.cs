using API.Hubs;
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BusinessLayer.Payload.RequestModel.PositionRequest;

namespace API.Controllers.PositionController
{
    [Route("api/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _service;
        private readonly IHubContext<SignalHub> _hubContext;
        public PositionController(IPositionService service, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequest request)
        {
            try
            {

                await _service.CreatePosition(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.POSITION_SIGNALR_MESSAGE.CREATED);
                return StatusCode(StatusCodes.Status201Created, "Position is created successfully");

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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionRequest request)
        {
            try
            {
                await _service.UpdatePositon(id, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.POSITION_SIGNALR_MESSAGE.UPDATED);
                return Ok("Position is updated successfully.");
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            try
            {
                await _service.DeletePosition(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.POSITION_SIGNALR_MESSAGE.DELETED);
                return Ok("Position is delete successfully.");
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetListPosition([FromQuery] PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetPositionList(paging, searchTerm, filterStatus);
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
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPositionDetail(int id)
        {
            try
            {
                var skill = await _service.GetPositionDetail(id);
                return Ok(skill);
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
