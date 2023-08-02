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
using System.IO;
using BusinessLayer.Utilities;

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
      
        [HttpGet]
        public async Task<IActionResult> GetExcelReportFile(string index, string url, int templateId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        byte[] excelData;

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            excelData = memoryStream.ToArray();
                        }

                        var updatedExcelData = await _service.ExportReportExcelFileFromUniversity(index, excelData, templateId);
                        return File(updatedExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportExcelFile.xlsx");
                    }
                }
                return NotFound();
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
