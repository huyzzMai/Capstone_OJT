using API.Hubs;
using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BusinessLayer.Payload.RequestModel.TemplateRequest;
using System.Linq;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;

namespace API.Controllers.TemplateController
{
    [Route("api/template")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _service;
        private readonly ITemplateHeaderService _headerservice;
        private readonly IHubContext<SignalHub> _hubContext;
        public TemplateController(ITemplateService service,ITemplateHeaderService headerService ,IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _headerservice = headerService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Admin")]
        [Route("template-header/criteriaheader/{templateId}")]
        [HttpGet]
        public async Task<IActionResult> GetListcriteriaheader(int templateId)
        {
            try
            {
                var list = await _headerservice.GetCriteriaTemplateHeader(templateId);
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
        [HttpGet]
        public async Task<IActionResult> GetListTemplate([FromQuery] PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetTemplateList(paging, searchTerm,filterStatus);
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
        [HttpPost]
        public async Task<IActionResult> Createtemplate([FromBody] CreateTemplateRequest request)
        {
            try
            {
                await _service.CreateTemplate(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATE_SIGNALR_MESSAGE.CREATED);
                return StatusCode(StatusCodes.Status201Created, "Template is created successfully");
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
        [Route("template-header/{templateId}")]
        [HttpPost]
        public async Task<IActionResult> Createtemplateheader(int templateId,[FromBody] CreateTemplateHeaderRequest request)
        {
            try
            {
                await _service.AddTemplateHeader(templateId,request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATEHEADER_SIGNALR_MESSAGE.CREATED);
                return StatusCode(StatusCodes.Status201Created, "Template header is add to template successfully");
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
        [Route("template-header/{templateId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTemplate(int templateId, [FromBody] UpdateTemplateHeaderRequest request)
        {
            try
            {
                await _service.UpdateTemplateHeader(templateId, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATEHEADER_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template header is updated successfully.");
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
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] UpdateTemplateRequest request)
        {
            try
            {
                await _service.UpdateTemplate(id, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATE_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template is updated successfully.");
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
        public async Task<IActionResult> GetDetailTemplateById(int id)
        {
            try
            {
                var cour = await _service.GetTemplateDetail(id);
                return Ok(cour);
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
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            try
            {
                await _service.DeleteTemplate(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATE_SIGNALR_MESSAGE.DELETED);
                return Ok("Template is delete successfully.");
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
