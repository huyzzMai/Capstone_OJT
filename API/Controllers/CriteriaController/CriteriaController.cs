//using API.Models.ResponseModel;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers.CriteriaController
{
    [Route("api/criteria")]
    [ApiController]
    public class CriteriaController : ControllerBase
    {
        private readonly IUserCriteriaService _service;

        public CriteriaController(IUserCriteriaService service)
        {
            _service = service;
        }

        [Authorize]
        [Route("list-of-trainee-point-by-trainer/{ojtBatchId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserCriteriaList(int ojtBatchId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var trainerId = int.Parse(userIdClaim.Value);
                var list = await _service.GetUserCriteriaToGrade(trainerId, ojtBatchId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize]
        [Route("list-current-point-of-trainee-point-by-trainer/{ojtBatchId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserCriteriaList(int ojtBatchId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var trainerId = int.Parse(userIdClaim.Value);
                var list = await _service.GetCurrentUserCriteria(trainerId, ojtBatchId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize]       
        [HttpPost]
        public async Task<IActionResult> UpdateUserCriteriaList([FromBody]List<UpdateCriteriaRequest> request)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var trainerId = int.Parse(userIdClaim.Value);
                await _service.UpdatePoints(trainerId, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

    }
}
