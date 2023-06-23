using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public PersonalUserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalInformation()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);

                var u = await userService.GetUserById(userId);
                var user = new PersonalUserResponse
                {
                    Email = u.Email,
                    FullName = u.Name,
                    Birthday = u.Birthday,
                    PhoneNumber = u.PhoneNumber,
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

                var u = await userService.GetUserById(userId);

                if (u == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
                }
                else
                {
                    await userService.UpdateUserInformation(userId, model);
                    //return Ok("Update successfully!");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
