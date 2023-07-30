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
using System.Net.Http;

namespace API.Controllers.ReportController
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;
        private readonly HttpClient _httpClient;

        public ReportController(IReportService service, IHttpClientFactory httpClientFactory)
        {
            _service= service;
            _httpClient = httpClientFactory.CreateClient();
        }
       
        //[Authorize(Roles = "Manager, Trainer")]
        //[HttpPost("export-excel-report-batch")]
        //public async Task<IActionResult> CreateReportInformation(int batchid)
        //{
        //    try
        //    {
        //        var data = await _service.CreateReportExcelFileFromBatch(batchid);                           
        //        return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FileReport.xlsx");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> GetExcelFile(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "YourExcelFile.xlsx"
                };
            }

            return NotFound(); // Or any other appropriate error response
        }
    }
}
