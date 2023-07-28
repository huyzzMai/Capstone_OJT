using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Utilities;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;

namespace API.Controllers.TaskController
{
    [Route("api/trainee-tasks")]
    [ApiController]
    [Authorize(Roles = "Trainee")]
    public class TraineeTaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public TraineeTaskController(ITaskService taskService, IUserService userService, IHubContext<SignalHub> hubContext)
        {
            this.taskService = taskService;
            this.userService = userService;
            _hubContext = hubContext;
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

        [HttpPost("{taskId}")]
        public async Task<IActionResult> UpdateFinishTask(string taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.CreateFinishTask(userId, taskId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_FINISH);
                return StatusCode(StatusCodes.Status201Created, "Accomplish task successfully.");
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
