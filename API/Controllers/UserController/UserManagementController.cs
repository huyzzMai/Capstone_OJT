using BusinessLayer.Models.ResponseModel.ExcelResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
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
    [Route("api/users")]
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

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("trainers")]
        public async Task<IActionResult> GetTrainerList()
        {
            try
            {
                return Ok(await userService.GetTrainerList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            try
            {
                return Ok(await userService.GetUserList());
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
        
        [Authorize(Roles = "Manager")]
        [HttpGet("trainees")]
        public async Task<IActionResult> GetTraineeList()
        {
            try
            {
                return Ok(await userService.GetTraineeList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet("trainers/trainees")]
        public async Task<IActionResult> GetTraineeListByTrainer()
        {
            try
            {
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await userService.GetTraineeListByTrainer(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
