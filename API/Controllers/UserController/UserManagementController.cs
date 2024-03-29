﻿using API.Hubs;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.UserRequest;
using BusinessLayer.Payload.ResponseModel.ExcelResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<SignalHub> _hubContext;

        public UserManagementController (IUserService userService, IAttendanceService attendanceService, IHubContext<SignalHub> hubContext)
        {
            this.userService = userService;
            _attendanceService = attendanceService;
            _hubContext = hubContext;   
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateUserRequest request)
        {
            try
            {
                await userService.CreateUser(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.CREATE);
                return StatusCode(StatusCodes.Status201Created,
                    "Create account successfully.");
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAccountList([FromQuery] PagingRequestModel paging,string searchTerm, int? role,int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetUserList(paging,searchTerm,role,filterStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountDetail(int id)
        {
            try
            {
                var user = await userService.GetUserDetail(id);
                return Ok(user);
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
        [HttpPut("disable-user/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            try
            {
                await userService.DisableUser(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.UPDATE);
                return Ok("User is updated successfully.");
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
        [HttpPut("active-user/{id}")]
        public async Task<IActionResult> ActiveUser(int id)
        {
            try
            {
                await userService.ActiveUser(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.UPDATE);
                return Ok("User is updated successfully.");
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
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("trainer")]
        public async Task<IActionResult> GetTrainerList([FromQuery] PagingRequestModel paging, [FromQuery] string keyword, [FromQuery] int? position)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetTrainerList(paging, keyword, position));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetTrainerDetail(int trainerId)
        {
            try
            {
                return Ok(await userService.GetTrainerDetail(trainerId));
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

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("trainee")]
        public async Task<IActionResult> GetTraineeList([FromQuery] PagingRequestModel paging, [FromQuery] string keyword, [FromQuery] int? position)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await userService.GetTraineeList(paging, keyword, position));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("unassigned-trainee")]
        public async Task<IActionResult> GetUnassignedTraineeList()
        {
            try
            {
                return Ok(await userService.GetUnassignedTraineeList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer,Manager")]
        [HttpGet("trainee/{traineeId}")]
        public async Task<IActionResult> GetTraineeDetail (int traineeId)
        {
            try
            {
                int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await userService.GetTraineeDetail(id, traineeId));
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

        [Authorize(Roles = "Manager")]
        [HttpPost("trainer/assign-trainees")]
        public async Task<IActionResult> AssignTraineeToTrainer([FromBody] AssignTraineesRequest request)
        {
            try
            {
                await userService.AssignTraineeToTrainer(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.ASSIGNED);
                return StatusCode(StatusCodes.Status201Created,
                    "Assign successfully.");
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

        [Authorize(Roles = "Manager")]
        [HttpGet("manager/trainee-list/{trainerId}")]
        public async Task<IActionResult> GetTraineeListByTrainerForManager([FromQuery] PagingRequestModel paging, int trainerId)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                //int id = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await userService.GetTraineeListByTrainer(trainerId, paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
