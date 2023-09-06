using API.Hubs;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BusinessLayer.Payload.RequestModel.UniversityRequest;
using DataAccessLayer.Commons;

namespace API.Controllers.UniversityController
{
    [Route("api/university")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _service;
        private readonly IHubContext<SignalHub> _hubContext;
        public UniversityController(IUniversityService service, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListUniversity([FromQuery] PagingRequestModel paging, string searchTerm,int? filterStaus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetUniversityList(paging, searchTerm,filterStaus);
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
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityRequest request)
        {
            try
            {
                await _service.CreateUniversuty(request);
                return StatusCode(StatusCodes.Status201Created, "University is created successfully");
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
        public async Task<IActionResult> GetUniversityDetail(int id)
        {
            try
            {
                var skill = await _service.GetDetailUniversityId(id);
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
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUniversity(int id, [FromBody] UpdateUniversityRequest request)
        {
            try
            {
                await _service.UpdateUniversity(id, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.UNIVERSITY_SIGNALR_MESSAGE.UPDATED);
                return Ok("University is updated successfully.");
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            try
            {
                await _service.DeleteUniversity(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.UNIVERSITY_SIGNALR_MESSAGE.DELETED);
                return Ok("University is deleted successfully.");
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
