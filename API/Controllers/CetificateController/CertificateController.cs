using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.CertificateRequest;
using System.Linq;

namespace API.Controllers.CetificateController
{
    [Route("api/certificate")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _service;
        public CertificateController(ICertificateService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPut("evaluation-certificate")]
        public async Task<IActionResult> EvaluateCertificate([FromBody] EvaluateCertificateRequest request)
        {
            try
            {
                await _service.EvaluateCertificate(request);
                return Ok("Certificate evaluate successfully.");
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
