using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel.ExcelRequest;
using DataAccessLayer.Models;
using System.Collections.Generic;
using static DataAccessLayer.Commons.CommonEnums;
using System.Linq;

namespace API.Controllers.ReportController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service= service;
        }

        [Authorize]
        [HttpGet("dataexcel")]
        public ActionResult<IEnumerable<string>> GetDataExcelValues()
        {
            var values = typeof(DataExcel)
                .GetFields()
                .Where(f => f.FieldType == typeof(string))
                .Select(f => f.GetValue(null))
                .Cast<string>()
                .ToList();

            return Ok(values);
        }
        [Authorize]
        [HttpPost("export-excel-report")]
        public async Task<IActionResult> CreateReportInformation([FromBody]ReportExcelRequest request)
        {
            try
            {
                var data = await _service.CreateReportGradeExcelFile(request);                           
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "fileReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
