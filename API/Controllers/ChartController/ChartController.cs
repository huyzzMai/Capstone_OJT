using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace API.Controllers.ChartController
{
    [Route("api/chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _service;

        public ChartController(IChartService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("batch-and-trainee/{year}")]
        public async Task<IActionResult> getBatchAndTrainee(int year)
        {
            try
            {
                var list = await _service.getBatchAndTrainee(year);
                return Ok(list);
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

        [Authorize(Roles = "Admin")]
        [HttpGet("trainer-with-most-trainees/{number}")]
        public async Task<IActionResult> getTrainerWithMostTrainees(int number)
        {
            try
            {
                var list = await _service.getTrainerWithMostTrainees(number);
                return Ok(list);
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

        [Authorize(Roles = "Admin")]
        [HttpGet("trainee-position")]
        public async Task<IActionResult> getTraineeByPosition()
        {
            try
            {
                var list = await _service.getTraineeByPosition();
                return Ok(list);
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
    }
}
