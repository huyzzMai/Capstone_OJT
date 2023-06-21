﻿using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;

namespace API.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]   
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await userService.LoginUser(request);
                var token = response.Token;
                if (token != null)
                {
                    //return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                    return StatusCode(StatusCodes.Status201Created,
                       new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
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