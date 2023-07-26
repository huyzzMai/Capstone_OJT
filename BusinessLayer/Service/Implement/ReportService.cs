using BusinessLayer.Service.Interface;
using ClosedXML.Excel;
using DataAccessLayer.Commons.CommonModels;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public byte[] GenerateExcel(string Name,List<UserCriteriaReport> reports)
        //{
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add(Name);

        //        // Add headers for UserCriteriaReport
        //        var userCriteriaReportHeaders = typeof(UserCriteriaReport).GetProperties();
        //        for (int i = 0; i < userCriteriaReportHeaders.Length; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = userCriteriaReportHeaders[i].Name;
        //            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        //        }

        //        // Add headers and fill in data for TemplatePoint properties
        //        var templatePointHeaders = reports.SelectMany(r => r.TemplatePoint).Select(tp => tp.Name).Distinct().ToList();
        //        for (int i = 0; i < templatePointHeaders.Count; i++)
        //        {
        //            worksheet.Cells[1, userCriteriaReportHeaders.Length + i + 1].Value = templatePointHeaders[i];
        //            worksheet.Cells[1, userCriteriaReportHeaders.Length + i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[1, userCriteriaReportHeaders.Length + i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[1, userCriteriaReportHeaders.Length + i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        //        }

        //        // Fill in the data
        //        for (int row = 0; row < reports.Count; row++)
        //        {
        //            var report = reports[row];
        //            var userCriteriaReportValues = userCriteriaReportHeaders.Select(h => h.GetValue(report));
        //            var templatePointValues = templatePointHeaders.Select(h =>
        //            {
        //                var point = report.TemplatePoint?.FirstOrDefault(tp => tp.Name == h)?.Point;
        //                return point != null ? point.ToString() : "N/A";
        //            });

        //            var rowValues = userCriteriaReportValues.Concat(templatePointValues?.Cast<object>() ?? Enumerable.Empty<object>());

        //            for (int col = 0; col < rowValues.Count(); col++)
        //            {
        //                worksheet.Cells[row + 2, col + 1].Value = rowValues.ElementAt(col);
        //            }
        //        }

        //        worksheet.DeleteColumn(7);

        //        // Auto-fit columns
        //        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        //        // Convert ExcelPackage to byte array
        //        return package.GetAsByteArray();
        //    }
        //}

        //public async Task<byte[]> CreateReportExcelFileFromBatch(int batchid)
        //{
        //    try
        //    {
        //        var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.Id == batchid);
        //        if (batch == null)
        //        {
        //            throw new Exception("Batch not found");
        //        }
        //        var trannee = await _unitOfWork.UserRepository.GetTraineeListByBatch(batchid);
        //        if (trannee == null || trannee.Count <1)
        //        {
        //            throw new Exception("Not found trainee in batch");
        //        }
        //        var lituser = await _unitOfWork.UserRepository.GetUserReportList(batchid, trannee);
        //        if (lituser == null || lituser.Count < 1)
        //        {
        //            throw new Exception("List not found");
        //        }
        //        var excel = GenerateExcel(batch.Name, lituser);

        //        return excel;
        //    } catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }           
        //}
    }
}
