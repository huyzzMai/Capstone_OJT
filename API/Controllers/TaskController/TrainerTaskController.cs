using API.Hubs;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<SignalHub> _hubContext;

        public TrainerTaskController(ITaskService taskService, IUserService userService, IHubContext<SignalHub> hubContext)
        {
            this.taskService = taskService;
            this.userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet("trainee/{traineeId}")]
        public async Task<IActionResult> GetListTaskPendingOfTrainee([FromQuery] PagingRequestModel paging, int traineeId)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListTaskPendingOfTrainee(userId, traineeId, paging));
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

        [HttpPut("task-accept/{taskId}")]
        public async Task<IActionResult> AcceptTraineeTask(int taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.AcceptTraineeTask(userId, taskId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_PROCESS);
                return Ok("Process task successfully.");
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

        [HttpPut("task-reject/{taskId}")]
        public async Task<IActionResult> DenyTraineeTask(int taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.RejectTraineeTask(userId, taskId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_PROCESS);
                return Ok("Process task successfully.");
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

        [HttpPost("board-webhook")]
        public async Task<IActionResult> AddBoardWebhook()
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.CreateBoardWebhook(userId));
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
