using API.Hubs;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        [HttpGet]
        public async Task<IActionResult> GetListAllTasksOfTrainee([FromQuery] PagingRequestModel paging, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListAllTaskOfTrainees(userId, paging, status));
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

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetTaskAccomplishedById(taskId, userId));
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

        [HttpGet("trainee/{traineeId}")]
        public async Task<IActionResult> GetListTaskOfTrainee([FromQuery] PagingRequestModel paging, [FromQuery] int? status, int traineeId)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListTaskOfTrainee(userId, traineeId, paging, status));
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

        [HttpGet("open-board")]
        public async Task<IActionResult> GetListCurrentOpenBoard()
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListOpenBoard(userId));
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

        [HttpGet("open-board/{boardId}/task-accomplished")]
        public async Task<IActionResult> GetListAccomplishedTaskOfBoard(string boardId, [FromQuery] PagingRequestModel paging, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await taskService.GetListTaskAccomplishOfBoard(boardId, paging, status));
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
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
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
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
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
