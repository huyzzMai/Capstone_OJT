using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Payload.RequestModel.CertificateRequest;
using System.Linq;
using BusinessLayer.Utilities;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Service.Implement;

namespace API.Controllers.CetificateController
{
    [Route("api/certificate")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _service;
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public CertificateController(ICertificateService service, IUserService userService, IHubContext<SignalHub> hubContext)
        {
            _service = service;
            this.userService = userService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet("trainer/{traineeId}")]
        public async Task<IActionResult> GetListCertificatesOfTraineeForTrainer(int traineeId, [FromQuery] PagingRequestModel paging, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await _service.GetListCertificateOfTraineeForTrainer(traineeId, paging, status));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet("trainer/{traineeId}/{courseId}")]
        public async Task<IActionResult> GetCertificateOfTraineeForTrainer(int traineeId, int courseId)
        {
            try
            {
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await _service.GetCertificateOfTraineeForTrainer(id, traineeId, courseId));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }


        [Authorize(Roles = "Trainer")]
        [HttpGet("trainer/pending-certificate")]
        public async Task<IActionResult> GetListCertificatePendingOffAllTraineesForTrainer([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await _service.GetListCertificatePendingOffAllTraineesForTrainer(id, paging));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainee")]
        [HttpGet]
        public async Task<IActionResult> GetListCertificatesOfTrainee([FromQuery] PagingRequestModel paging, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await _service.GetListCertificateOfTrainee(id, paging, status));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainee")]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCertificateOfTrainee(int courseId)
        {
            try
            {
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await _service.GetCertificateOfTrainee(id, courseId));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("trainer/valid-certificate")]
        public async Task<IActionResult> AcceptCertificate([FromBody] EvaluateCertificateRequest request)
        {
            try
            {
                await _service.AcceptCertificate(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.CERTIFICATE_MESSAGE.PROCESS_CERTIFICATE);
                return Ok("Certificate evaluate successfully.");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("trainer/invalid-certificate")]
        public async Task<IActionResult> DenyCertificate([FromBody] EvaluateCertificateRequest request)
        {
            try
            {
                await _service.DenyCertificate(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.CERTIFICATE_MESSAGE.PROCESS_CERTIFICATE);
                return Ok("Certificate evaluate successfully.");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize]
        [HttpPut("submition-certificate")]
        public async Task<IActionResult> SubmitCertificate([FromBody] SubmitCertificateRequest request)
        {
            try
            {
                var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await _service.SubmitCertificate(userid, request);
                return Ok("Certificate submit successfully.");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize]
        [HttpPut("resubmition-certificate")]
        public async Task<IActionResult> ReSubmitCertificate([FromBody] SubmitCertificateRequest request)
        {
            try
            {
                var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await _service.ReSubmitCertificate(userid, request);
                return Ok("Certificate resubmit successfully.");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
