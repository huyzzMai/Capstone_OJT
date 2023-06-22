﻿using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.UserController
{
    [Route("api/users")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService userService;

        public UserManagementController (IUserService userService)
        {
            this.userService = userService; 
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("trainers")]
        public async Task<IActionResult> GetTrainerList()
        {
            try
            {
                return Ok(await userService.GetTrainerList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            try
            {
                return Ok(await userService.GetUserList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("trainees")]
        public async Task<IActionResult> GetTraineeList()
        {
            try
            {
                return Ok(await userService.GetTraineeList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }


    }
}
