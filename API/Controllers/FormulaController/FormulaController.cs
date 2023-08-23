using API.Hubs;
using BusinessLayer.Models.RequestModel.SkillRequest;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel.EnumResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BusinessLayer.Models.RequestModel.FormulaRequest;

namespace API.Controllers.Formula
{
    [Route("api/formula")]
    [ApiController]
    public class FormulaController : ControllerBase
    {
        private readonly IFormulaService _service;      
        public FormulaController(IFormulaService service)
        {
            _service = service;         
        }
        [Authorize]
        [HttpGet("enum-operand")]
        public IActionResult Get()
        {
            var enums = typeof(EnumData)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .Where(t => t.IsEnum)
                .Select(t => new
                {
                    Name = t.Name,
                    Values = Enum.GetNames(t)
                })
                .ToList();
            return Ok(enums);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFormula([FromBody] CreateFormulaRequest request)
        {
            try
            {

                await _service.CreateFormula(request);               
                return StatusCode(StatusCodes.Status201Created, "Formula is created successfully");

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
        public async Task<IActionResult> UpdateFormula(int id, [FromBody] UpdateFormulaRequest request)
        {
            try
            {
                await _service.UpdateFormula(id, request);
                return Ok("Formula is updated successfully.");
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
        public async Task<IActionResult> DeleteFormula(int id)
        {
            try
            {
                await _service.DeleteFormula(id);
                return Ok("Formula is delete successfully.");
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
        public async Task<IActionResult> GetListFormula([FromQuery] PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var list = await _service.GetFormulaList(paging, searchTerm, filterStatus);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormulaDetail(int id)
        {
            try
            {
                var formular = await _service.GetFormulaDetail(id);
                return Ok(formular);
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
