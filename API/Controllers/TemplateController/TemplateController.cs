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
        public TemplateController(ITemplateService service, ITemplateHeaderService headerService, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _headerservice = headerService;
            _hubContext = hubContext;
        }
      

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetListTemplate([FromQuery] PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetTemplateList(paging, searchTerm, filterStatus);
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
        [Route("list-active-template-by-university/{universityId}")]
        public async Task<IActionResult> GetListUniversityTemplate(int universityId)
        {
            try
            {
               
                var list = await _service.GetTemplateUniversityList(universityId);
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

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
        [HttpPut("active-template/{id}")]
        public async Task<IActionResult> ActiveTemplate(int id)
        {
            try
            {
                await _service.ActiveTemplate(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATE_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template is active successfully.");
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

        [Authorize(Roles = "Manager,Trainer")]
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

        [Authorize(Roles = "Manager")]     
        [Route("template-header/{id}")]
        [HttpPost]
        public async Task<IActionResult> Createtemplateheader(int id,[FromBody] CreateTemplateHeaderRequest request)
        {
            try
            {
                await _headerservice.AddTemplateHeader(id,request);
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
        [Authorize(Roles = "Manager")]      
        [Route("template-header/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] UpdateTemplateHeaderRequest request)
        {
            try
            {
                await _headerservice.UpdateTemplateHeader(id, request);
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
        [Authorize(Roles = "Manager")]
        [HttpPut("disable-template/{id}")]
        public async Task<IActionResult> DisableTemplate(int id)
        {
            try
            {
                await _service.DisableTemplate(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATE_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template is disable successfully.");
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
        [Authorize(Roles = "Manager")]
        [HttpPut("template-header/disable-template/{id}")]
        public async Task<IActionResult> DisableTemplateHeader(int id)
        {
            try
            {
                await _headerservice.DisableTemplateHeader(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATEHEADER_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template header is disable successfully.");
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
        [Authorize(Roles = "Manager")]
        [HttpPut("template-header/active-template/{id}")]
        public async Task<IActionResult> ActiveTemplateHeader(int id)
        {
            try
            {
                await _headerservice.ActiveTemplateHeader(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TEMPLATEHEADER_SIGNALR_MESSAGE.UPDATED);
                return Ok("Template header is active successfully.");
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
