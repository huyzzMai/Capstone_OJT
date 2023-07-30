using API.Hubs;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.SkillRequest;
using BusinessLayer.Models.RequestModel;

namespace API.Controllers.SkillController
{
    [Route("api/skill")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _service;
        private readonly IHubContext<SignalHub> _hubContext;
        public SkillController(ISkillService service, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillRequest request)
        {
            try
            {

                await _service.CreateSkill(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.SKILL_SIGNALR_MESSAGE.CREATED);
                return StatusCode(StatusCodes.Status201Created, "Skill is created successfully");

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
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] UpdateSkillRequest request)
        {
            try
            {
                await _service.UpdateSkill(id, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.SKILL_SIGNALR_MESSAGE.UPDATED);
                return Ok("Skill is updated successfully.");
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
        public async Task<IActionResult> DeleteSkill(int id)
        {
            try
            {
                await _service.DeleteSkill(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.SKILL_SIGNALR_MESSAGE.DELETED);
                return Ok("Skill is delete successfully.");
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
        public async Task<IActionResult> GetListSkill([FromQuery] PagingRequestModel paging,string searchTerm)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetSkillList(paging,searchTerm);
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
        public async Task<IActionResult> GetSkillDetail(int id)
        {
            try
            {
               var skill= await _service.GetSkillDetail(id);
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
