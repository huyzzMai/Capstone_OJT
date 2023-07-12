using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel.ExcelResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.UserController
{
    [Route("api/user")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAttendanceService _attendanceService;

        public UserManagementController (IUserService userService, IAttendanceService attendanceService)
        {
            this.userService = userService;
            _attendanceService = attendanceService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateUserRequest request)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created,
                    await userService.CreateUser(request));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAccountList([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetUserList(paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("getattendanceformfile")]
        public async Task<IActionResult> UploadAttendanceFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var filePath = await _attendanceService.SaveTempFile(file); // Save the file temporarily on the server
                var attendanceData = await _attendanceService.ProcessAttendanceFile(filePath); // Pass the file path to the service
                return Ok(attendanceData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("trainer")]
        public async Task<IActionResult> GetTrainerList([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetTrainerList(paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("trainee")]
        public async Task<IActionResult> GetTraineeList([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetTraineeList(paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("trainer/{trainerId}/trainee/{traineeId}")]
        public async Task<IActionResult> AssignTraineeToTrainer(int trainerId, int traineeId)
        {
            try
            {
                await userService.AssignTraineeToTrainer(trainerId, traineeId);
                return StatusCode(StatusCodes.Status201Created,
                    "Assign successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet("trainer/trainee")]
        public async Task<IActionResult> GetTraineeListByTrainer([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await userService.GetTraineeListByTrainer(id, paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
