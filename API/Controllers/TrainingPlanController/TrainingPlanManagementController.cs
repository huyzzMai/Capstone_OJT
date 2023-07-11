using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.TrainingPLanRequest;

namespace API.Controllers.TrainingPlanController
{
    [Route("api/training-plan")]
    [ApiController]
    public class TrainingPlanManagementController : ControllerBase
    {
        private readonly ITrainingPlanService trainingService;
        private readonly IUserService userService;

        public TrainingPlanManagementController(ITrainingPlanService trainingService, IUserService userService)
        {
            this.trainingService = trainingService;
            this.userService = userService;
        }

        // API for all Role

        [Authorize(Roles = "Manager, Trainer, Trainee")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingPlanAllRole(int id)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await trainingService.GetTrainingPlanForAllRole(userId, id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }


        // API for Manager

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetTrainingPlanList()
        {
            try
            {
                return Ok(await trainingService.GetTrainingPlanList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("verification-accept/{id}")]
        public async Task<IActionResult> AcceptTrainingPlan(int id)
        {
            try
            {
                await trainingService.AcceptTrainingPlan(id);
                return Ok("Training plan is accepted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("verification-deny/{id}")]
        public async Task<IActionResult> DenyTrainingPlan(int id)
        {
            try
            {
                await trainingService.DenyTrainingPlan(id);
                return Ok("Training plan is denied.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }


        // API for Trainer

        [Authorize(Roles = "Trainer")]
        [HttpGet("owner")]
        public async Task<IActionResult> GetTrainingPlanListOfOwner()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await trainingService.GetTrainingPlanListByOwner(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        public async Task<IActionResult> CreateTrainingPlan([FromBody] CreateTrainingPlanRequest request)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.CreateTrainingPlan(userId, request);
                return StatusCode(StatusCodes.Status201Created,"Training plan is created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingPlan(int id, [FromBody] UpdateTrainingPlanRequest request)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.UpdateTrainingPlan(userId, id, request);
                return Ok("Training plan is updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost("detail/{planId}")]
        public async Task<IActionResult> CreateTrainingPlanDetailForExistingTrainingPlan(int planId, [FromBody] CreateTrainingPlanDetailRequest request)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.CreateTrainingPlanDetailForExistingTrainingPlan(userId, planId, request);
                return StatusCode(StatusCodes.Status201Created, "Training plan detail is created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("detail/{id}")]
        public async Task<IActionResult> UpdateTrainingPlanDetail(int id, [FromBody] CreateTrainingPlanDetailRequest request)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.UpdateTrainingPlanDetail(userId, id, request);
                return Ok("Update training plan detail successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("deactivated-plan/{planId}")]
        public async Task<IActionResult> DeactivateTrainingPlan(int planId)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.DeactivateTrainingPlan(userId, planId);
                return Ok("Training plan is deactivated.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("open-plan/{planId}")]
        public async Task<IActionResult> OpenDeactivatedTrainingPlan(int planId)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.OpenDeactivatedTrainingPlan(userId, planId);
                return Ok("Training plan is re-opened.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost("{id}/trainee/{traineeId}")]
        public async Task<IActionResult> AssignTraineeToTrainingPlan(int id, int traineeId)
        {
            try
            {
                // Get id of current log in user 
                int trainerId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.AssignTraineeToTrainingPlan(trainerId, traineeId, id);
                return StatusCode(StatusCodes.Status201Created, "Assign trainee to training plan successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
