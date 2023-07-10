using BusinessLayer.Models.RequestModel.TrainingPLanRequest;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Models.RequestModel.CourseRequest;

namespace API.Controllers.CourseController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
           _service= service;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            try
            {
              
                await _service.CreateCourse(request);
                return StatusCode(StatusCodes.Status201Created, "Course is created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            try
            {
                // Get id of current log in user 
                await _service.UpadateCourse(id, request);
                return Ok("Course is updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                // Get id of current log in user 
                await _service.DeleteCourse(id);
                return Ok("Course is delete successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListCourse()
        {
            try
            {
               var list= await _service.GetCourseList();
                return Ok(list);

            } catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }

        [Authorize]
        [HttpGet("recommendedList/{userid}")]
        public async Task<IActionResult> GetListCourseRecommendForUser(int userid)
        {
            try
            {
                var list = await _service.GetCourserecommendListForUser(userid);
                return Ok(list);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }

        [Authorize]
        [HttpGet("compulsoryList")]
        public async Task<IActionResult> GetListCourseCompulsoryForUser()
        {
            try
            {
                var list = await _service.GetCourseCompulsoryForUser();
                return Ok(list);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }
    }
}
