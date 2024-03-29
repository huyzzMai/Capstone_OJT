﻿using API.Hubs;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.TrainingPLanRequest;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.Controllers.TrainingPlanController
{
    [Route("api/training-plan")]
    [ApiController]
    public class TrainingPlanManagementController : ControllerBase
    {
        private readonly ITrainingPlanService trainingService;
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public TrainingPlanManagementController(ITrainingPlanService trainingService, IUserService userService, IHubContext<SignalHub> hubContext)
        {
            this.trainingService = trainingService;
            this.userService = userService;
            _hubContext = hubContext;
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

        [Authorize(Roles = "Trainee")]
        [HttpGet("personal-trainee")]
        public async Task<IActionResult> GetTrainingPlanForPersonalTrainee()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await trainingService.GetTrainingPlanForTrainee(userId));
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


        // API for Manager

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetTrainingPlanList([FromQuery] PagingRequestModel paging, [FromQuery] string nameSearch, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                return Ok(await trainingService.GetTrainingPlanList(paging, nameSearch, status));
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

        //[Authorize(Roles = "Manager")]
        //[HttpGet("pending-training-plans")]
        //public async Task<IActionResult> GetTrainingPlanListPending([FromQuery] PagingRequestModel paging, [FromQuery] string nameSearch, [FromQuery] int? status)
        //{
        //    try
        //    {
        //        paging = PagingUtil.checkDefaultPaging(paging);
        //        return Ok(await trainingService.GetTrainingPlanListPending(paging, nameSearch, status));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        [Authorize(Roles = "Manager")]
        [HttpPut("verification-accept/{id}")]
        public async Task<IActionResult> AcceptTrainingPlan(int id)
        {
            try
            {
                await trainingService.AcceptTrainingPlan(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.PROCESS);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
                return Ok("Training plan is accepted.");
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

        [Authorize(Roles = "Manager")]
        [HttpPut("verification-deny/{id}")]
        public async Task<IActionResult> DenyTrainingPlan(int id)
        {
            try
            {
                await trainingService.DenyTrainingPlan(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.PROCESS);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
                return Ok("Training plan is denied.");
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


        // API for Trainer

        [Authorize(Roles = "Trainer")]
        [HttpGet("owner")]
        public async Task<IActionResult> GetTrainingPlanListOfOwner([FromQuery] PagingRequestModel paging, [FromQuery] string nameSearch, [FromQuery] int? status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await trainingService.GetTrainingPlanListByOwner(userId, paging, nameSearch, status));
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

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        public async Task<IActionResult> CreateTrainingPlan([FromBody] CreateTrainingPlanRequest request)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.CreateTrainingPlan(userId, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.CREATE);
                return StatusCode(StatusCodes.Status201Created, "Training plan is created successfully");
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
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.UPDATE);
                return Ok("Training plan is updated successfully.");
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

        //[Authorize(Roles = "Trainer")]
        //[HttpPost("detail/{planId}")]
        //public async Task<IActionResult> CreateTrainingPlanDetailForExistingTrainingPlan(int planId, [FromBody] List<CreateTrainingPlanDetailRequest> request)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.CreateTrainingPlanDetailForExistingTrainingPlan(userId, planId, request);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.UPDATE);
        //        return StatusCode(StatusCodes.Status201Created, "Training plan detail is created successfully.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Trainer")]
        //[HttpPut("detail/{id}")]
        //public async Task<IActionResult> UpdateTrainingPlanDetail(int id, [FromBody] CreateTrainingPlanDetailRequest request)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.UpdateTrainingPlanDetail(userId, id, request);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.UPDATE);
        //        return Ok("Update training plan detail successfully.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Trainer")]
        //[HttpPut("deactivated-plan/{planId}")]
        //public async Task<IActionResult> DeactivateTrainingPlan(int planId)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.DeactivateTrainingPlan(userId, planId);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.UPDATE);
        //        return Ok("Training plan is deactivated.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Trainer")]
        //[HttpPut("open-plan/{planId}")]
        //public async Task<IActionResult> OpenDeactivatedTrainingPlan(int planId)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.OpenDeactivatedTrainingPlan(userId, planId);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.UPDATE);
        //        return Ok("Training plan is re-opened.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        [Authorize(Roles = "Trainer")]
        [HttpPost("assign-trainee")]
        public async Task<IActionResult> AssignTraineeToTrainingPlan([FromBody] AssignTrainingPlanForTraineeRequest request)
        {
            try
            {
                // Get id of current log in user 
                int trainerId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await trainingService.AssignTraineeToTrainingPlan(trainerId, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.ASSIGN);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
                return StatusCode(StatusCodes.Status201Created, "Assign trainee to training plan successfully.");
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

        //[Authorize(Roles = "Trainer")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTrainingPlan(int id)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int trainerId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.DeleteTrainingPlan(id, trainerId);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.DELETE);
        //        return Ok("Training plan is deleted.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Trainer")]
        //[HttpDelete("detail/{id}")]
        //public async Task<IActionResult> DeleteTrainingPlanDetail(int id)
        //{
        //    try
        //    {
        //        // Get id of current log in user 
        //        int trainerId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await trainingService.DeleteTrainingPlanDetail(id, trainerId);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TRAINING_PLAN_MESSAGE.DETAIL_DELETE);
        //        return Ok("Training plan detail is deleted.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}
    }
}
