﻿using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Utilities;

namespace API.Controllers.TaskController
{
    [Route("api/trainee-tasks")]
    [ApiController]
    [Authorize(Roles = "Trainee")]
    public class TraineeTaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IUserService userService;

        public TraineeTaskController(ITaskService taskService, IUserService userService)
        {
            this.taskService = taskService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskOfTrainee([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetAllTaskOfTrainee(userId, paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("accomplished-tasks")]
        public async Task<IActionResult> GetListFinishTaskOfTrainee([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListTaskAccomplished(userId, paging));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("{taskId}")]
        public async Task<IActionResult> UpdateFinishTask(string taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.CreateFinishTask(userId, taskId);
                return StatusCode(StatusCodes.Status201Created, "Accomplish task successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
