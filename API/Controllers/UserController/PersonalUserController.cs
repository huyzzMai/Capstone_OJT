using API.Hubs;
using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel.UserResponse;
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

                var u = await userService.GetCurrentUserById(userId);
                var user = new PersonalUserResponse
                {
                    Email = u.Email,
                    FullName = u.Name,
                    Birthday = u.Birthday,
                    Gender = u.Gender ?? default(int),
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,    
                    AvatarURL = u.AvatarURL,
                    RollNumber = u.RollNumber,
                    Position = u.Position ?? default(int)
                };

                return Ok(user);
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

                var u = await userService.GetCurrentUserById(userId);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await userService.UpdateUserInformation(userId, model);
                    await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.USER_MESSAGE.UPDATE);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
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
