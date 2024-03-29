﻿using BusinessLayer.Payload.ResponseModel.UserResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Utilities;
using BusinessLayer.Payload.RequestModel.OjtBatchRequest;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CourseRequest;
using System.Linq;

namespace API.Controllers.OJTBatchController
{
    [Route("api/ojtbatch")]
    [ApiController]
    public class OJTBatchController : ControllerBase
    {
        private readonly IOJTBatchService _ojtService;
        private readonly IHubContext<SignalHub> _hubContext;
        public OJTBatchController(IOJTBatchService ojtService, IHubContext<SignalHub> hubContext)
        {
            _ojtService = ojtService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("ongoing-batches")]
        public async Task<IActionResult> GetValidOJTBatchInformation([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _ojtService.GetValidOJtList(paging);
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
        [Authorize(Roles = "Trainer")]
        [HttpGet]
        [Route("status-grade-batches-trainer")]
        public async Task<IActionResult> GetStatusGradeBatch()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var list = await _ojtService.getListGradePointOjtbatch(int.Parse(userIdClaim.Value));
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
        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("export-status-batches")]
        public async Task<IActionResult> GetOjtStatusExport([FromQuery] PagingRequestModel paging,string searchTerm ,string status)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _ojtService.getListOjtbatchExportStatus(paging,searchTerm, status);
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
        [Authorize(Roles = "Admin,Trainer")]
        [HttpGet("{id}")]        
        public async Task<IActionResult> GetDetailOJTBatchInformation(int id)
        {
            try
            {
                var list = await _ojtService.GetDetailOjtBatch(id);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOjtBatch(int id, [FromBody] UpdateOjtBatchRequest request)
        {
            try
            {
                await _ojtService.UpdateOjtBatch(id, request);              
                return Ok("Ojt batch is updated successfully.");
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
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOjtBacth(int id)
        //{
        //    try
        //    {
        //        await _ojtService.DeleteOjtBatch(id);
        //        return Ok("Ojt batch is delete successfully.");
        //    }
        //    catch (ApiException ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //          e.Message);
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOJTBatchInformation([FromBody]CreateOjtBatchRequest request)
        {
            try
            {
                await _ojtService.CreateOjtBatch(request);              
                return StatusCode(StatusCodes.Status201Created, "Batch is created successfully");
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("batches-of-university/{universityid}")]
        public async Task<IActionResult> GetValidOJTBatchInformation(int universityid, [FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _ojtService.GetValidOJtListbyUniversityId(universityid,paging);
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
