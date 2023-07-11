using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace API.Controllers.TaskController
{
    [Route("api/task")]
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

        [HttpGet("trainee-tasks")]
        public async Task<IActionResult> GetAllTaskOfTrainee()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetAllTaskOfTrainee(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("trainee-tasks/accomplished-tasks")]
        public async Task<IActionResult> GetListFinishTaskOfTrainee()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListTaskAccomplished(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("trainee-tasks/{taskId}")]
        public async Task<IActionResult> UpdateFinishTask(string taskId)
        {
            int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
            await taskService.UpdateFinishTask(userId, taskId);
            return StatusCode(StatusCodes.Status201Created, "Accomplish task successfully.");
        }
    }
}
