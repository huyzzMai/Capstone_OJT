using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Models.RequestModel.CourseRequest;
using System.Linq;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Utilities;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;

namespace API.Controllers.CourseController
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IHubContext<SignalHub> _hubContext;
        public CourseController(ICourseService service, IHubContext<SignalHub> hubContext)
        {
           _service= service;
           _hubContext= hubContext;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            try
            {              
                await _service.CreateCourse(request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.COURSE_SIGNALR_MESSAGE.CREATED);
                return StatusCode(StatusCodes.Status201Created, "Course is created successfully");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            try
            {
                await _service.UpdateCourse(id, request);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.COURSE_SIGNALR_MESSAGE.UPDATED);
                return Ok("Course is updated successfully.");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailCourseById(int id)
        {
            try
            {
                var cour = await _service.GetDetailCoursebyId(id);
                return Ok(cour);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("course-participation")]
        public async Task<IActionResult> EnrollCourse(int courseid)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                await _service.EnrollCourse(int.Parse(userIdClaim.Value), courseid);
                return Ok("Enroll successfully.");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {             
                await _service.DeleteCourse(id);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.COURSE_SIGNALR_MESSAGE.DELETED);
                return Ok("Course is delete successfully.");
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListCourse([FromQuery] PagingRequestModel paging, string sortField, string sortOrder, string searchTerm)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetCourseList(paging,sortField,sortOrder,searchTerm);
                return Ok(list);

            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpGet("attendance-courses")]
        public async Task<IActionResult> GetEnrollCourse([FromQuery] PagingRequestModel paging)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetEnrollCourse(int.Parse(userIdClaim.Value),paging);
                return Ok(list);

            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
        [Authorize]
        [HttpGet("recommendation-courses")]
        public async Task<IActionResult> GetListCourseRecommendForUser([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var list = await _service.GetCourserecommendListForUser(int.Parse(userIdClaim.Value),paging);
                return Ok(list);

            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }

        [Authorize]
        [HttpGet("compulsory-courses")]
        public async Task<IActionResult> GetListCourseCompulsoryForUser([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var list = await _service.GetCourseCompulsoryForUser(int.Parse(userIdClaim.Value),paging);
                return Ok(list);

            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  e.Message);
            }
        }
    }
}
