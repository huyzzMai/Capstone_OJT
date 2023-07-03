using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using OfficeOpenXml;
using System.Data;

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
        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            // Tạo DataTable với dữ liệu
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));

            // Thêm dữ liệu vào DataTable
            dataTable.Rows.Add(1, "John Doe");
            dataTable.Rows.Add(2, "Jane Smith");
            dataTable.Rows.Add(3, "Mike Johnson");

            // Tạo tệp Excel mới và lấy sheet đầu tiên
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Đổ dữ liệu từ DataTable vào sheet
                worksheet.Cells.LoadFromDataTable(dataTable, true);

                // Lưu tệp Excel
                byte[] excelBytes = package.GetAsByteArray();

                // Trả về tệp Excel như là một File trực tiếp
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
        }
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

        [HttpPost("reset-password-code")]
        public async Task<IActionResult> SendForgetPasswordCode(string email)
        {
            try
            {
                await userService.SendTokenResetPassword(email);
                return Ok("Reset code sent to your email.");
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        // After successfully verify reset code, go to this API
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                await userService.ResetPassword(request);
                return Ok("Reset password successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
