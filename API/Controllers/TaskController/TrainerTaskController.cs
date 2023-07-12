using BusinessLayer.Models.RequestModel;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.TaskController
{
    [Route("api/task-process")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class TrainerTaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IUserService userService;

        public TrainerTaskController(ITaskService taskService, IUserService userService)
        {
            this.taskService = taskService;
            this.userService = userService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetListTaskPendingOfTrainee([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("task-accept/{taskId}")]
        public async Task<IActionResult> AcceptTraineeTask(string taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.AcceptTraineeTask(userId, taskId);

                return Ok("Process task successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("task-reject/{taskId}")]
        public async Task<IActionResult> DenyTraineeTask(string taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.RejectTraineeTask(userId, taskId);

                return Ok("Process task successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
