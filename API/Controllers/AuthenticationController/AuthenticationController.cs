using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Utilities;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using DataAccessLayer.Commons;

namespace API.Controllers.AuthenticationController
{
    [Route("api/authen")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await userService.LoginUser(request);

                var result = new TokenResponse();
                //bool i = true;
                // xài while check trùng refToken
                //while (i == true)
                //{
                //    var tokenResponse = await userService.CreateToken(response.UserId, response.Role);
                //    var check = await userService.CheckExistUserRefToken(tokenResponse.Token);
                //    result = tokenResponse;
                //    i = check;
                //}
                var tokenResponse = await userService.CreateToken(response.UserId, response.Role);
                result = tokenResponse;

                await userService.SaveUserRefToken(Int32.Parse(response.UserId), result.RefreshToken);

                var token = result.Token;
                if (result.Token != null && result.RefreshToken != null)
                {
                    return StatusCode(StatusCodes.Status201Created,
                    result);
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var user = await userService.GetUserByRefToken(request.RefreshToken);
                if (user == null)
                {
                    return BadRequest("Invalid refresh token.");
                }
                string role = null;
                var userId = user.Id.ToString();
                if (user.Role == CommonEnums.ROLE.ADMIN)
                {
                    role = "Admin";
                }
                else if (user.Role == CommonEnums.ROLE.MANAGER)
                {
                    role = "Manager";
                }
                else if (user.Role == CommonEnums.ROLE.TRAINER)
                {
                    role = "Trainer";
                }
                else if (user.Role == CommonEnums.ROLE.TRAINEE)
                {
                    role = "Trainee";
                }

                var result = new TokenResponse();
                //bool i = true;
                //// xài while check trùng refToken
                //while (i == true)
                //{
                //    var tokenResponse = await userService.CreateToken(userId, role);
                //    var check = await userService.CheckExistUserRefToken(tokenResponse.Token);
                //    result = tokenResponse;
                //    i = check;
                //}
                var tokenResponse = await userService.CreateToken(userId, role);
                result = tokenResponse;

                await userService.SaveUserRefToken(user.Id, result.RefreshToken);

                var token = result.Token;
                if (result.Token != null && result.RefreshToken != null)
                {
                    return StatusCode(StatusCodes.Status201Created,
                    result);
                }
                else
                {
                    return BadRequest("Invalid Credentials");
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

        [HttpPost("reset-password-code/{email}")]
        public async Task<IActionResult> SendForgetPasswordCode(string email)
        {
            try
            {
                await userService.SendTokenResetPassword(email);
                return Ok("Reset code sent to your email.");
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

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] ResetCodeRequest request)
        {
            try
            {
                await userService.VerifyResetCode(request.ResetCode);
                return Ok("Reset code correct.");
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

        // After successfully verify reset code, go to this API
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                await userService.ResetPassword(request);
                return Ok("Reset password successfully!");
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
