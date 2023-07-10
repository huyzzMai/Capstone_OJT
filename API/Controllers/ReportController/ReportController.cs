using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccessLayer.Models;
using System.Collections.Generic;
using static DataAccessLayer.Commons.CommonEnums;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Vml.Office;

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
       
        [Authorize(Roles = "Manager, Trainer")]
        [HttpPost("export-excel-report-batch")]
        public async Task<IActionResult> CreateReportInformation(int batchid)
        {
            try
            {
                var data = await _service.CreateReportExcelFileFromBatch(batchid);                           
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FileReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
