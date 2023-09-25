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
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.RequestModel;

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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPut("disable-skill/{id}")]
        public async Task<IActionResult> DisableSkill(int id)
        {
            try
            {
                await _service.DisableSkill(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.SKILL_SIGNALR_MESSAGE.UPDATED);
                return Ok("Skill is disable successfully.");
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
        [HttpPut("active-skill/{id}")]
        public async Task<IActionResult> ActiveSkill(int id)
        {
            try
            {
                await _service.ActiveSkill(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.SKILL_SIGNALR_MESSAGE.UPDATED);
                return Ok("Skill is active successfully.");
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
        public async Task<IActionResult> GetListSkill([FromQuery] PagingRequestModel paging,string searchTerm,int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetSkillList(paging,searchTerm, filterStatus);
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
