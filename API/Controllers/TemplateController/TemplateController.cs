using API.Hubs;
using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.TemplateRequest;

namespace API.Controllers.TemplateController
{
    [Route("api/template")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _service;
        private readonly IHubContext<SignalHub> _hubContext;
        public TemplateController(ITemplateService service, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }
        [Authorize]
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
    }
}
