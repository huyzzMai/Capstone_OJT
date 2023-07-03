using BusinessLayer.Models.RequestModel.ExcelRequest;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IReportService
    {
       Task<byte[]> CreateReportGradeExcelFile(ReportExcelRequest data);
    }
}
