using API.Hubs;
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel.EnumResponse;
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
using BusinessLayer.Payload.RequestModel.FormulaRequest;
using BusinessLayer.Service;

namespace API.Controllers.Formula
{
    [Route("api/formula")]
    [ApiController]
    public class FormulaController : ControllerBase
    {
        private readonly IFormulaService _service;
        private readonly OperandService _operandService;
        public FormulaController(IFormulaService service, OperandService operandService)
        {
            _service = service;
            _operandService = operandService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("data-operand")]
        public IActionResult Get([FromQuery] string category)
        {
            if (category == null)
            {
                var allOperands = _operandService.GetAllOperands();
                return Ok(allOperands);
            }
            var operands = _operandService.GetOperandsByKey(category);
            if (operands != null)
            {
                return Ok(operands);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]    
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
