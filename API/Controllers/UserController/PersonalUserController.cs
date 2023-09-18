using API.Hubs;
using BusinessLayer.Payload.RequestModel.UserRequest;
using BusinessLayer.Payload.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Controllers.UserController
{
    [Route("api/personal-user")]
    [ApiController]
    //[Authorize(Roles = "Manager,Trainer,Trainee")]
    [Authorize]
    public class PersonalUserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public PersonalUserController(IUserService userService, IHubContext<SignalHub> hubContext)
        {
            this.userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalInformation()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var u = await userService.GetUserProfile(userId);
                return Ok(u);
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

        [HttpPut]
        public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserInformationRequest model)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await userService.UpdateUserInformation(userId, model);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.UPDATE);
                return StatusCode(StatusCodes.Status204NoContent);
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

        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordRequest model)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await userService.UpdateUserPassword(userId, model);
                return StatusCode(StatusCodes.Status204NoContent);  
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
