﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Service.Interface;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Utilities;

namespace API.Controllers.AttendanceController
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IHubContext<SignalHub> _hubContext;

        public AttendanceController(IAttendanceService attendanceService, IHubContext<SignalHub> hubContext)
        {
            _attendanceService = attendanceService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("data-of-file-attendance")]
        public async Task<IActionResult> UploadAttendanceFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            try
            {                             
                var attendanceData = await _attendanceService.ProcessAttendanceFile(file);
                return Ok(attendanceData);
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
        [HttpGet("attendance-by-month/{month}/{year}")]
        public async Task<IActionResult> GetAttendanceByMonth(int month, int year)
        {           
            try
            {
                
                var attendanceData = await _attendanceService.GetAttendanceByMonth(month, year);
                return Ok(attendanceData);
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
        [HttpGet("attendance-by-date/{date}")]
        public async Task<IActionResult> GetAttendanceByMonth(DateTime date)
        {
            try
            {
                var attendanceData = await _attendanceService.GetAttendanceByDate(date);
                return Ok(attendanceData);
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
