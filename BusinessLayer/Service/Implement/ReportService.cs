using BusinessLayer.Models.RequestModel.ExcelRequest;
using BusinessLayer.Service.Interface;
using ClosedXML.Excel;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<byte[]> CreateReportGradeExcelFile(ReportExcelRequest data)
        {
            using (var workbook = new XLWorkbook())
            {
                var namesheet = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.Id == data.BatchId);
                var worksheet = workbook.Worksheets.Add(namesheet.Name);

                var pairdata = data.PairDataExcel.ToList();
                // Add headers
                var headers = pairdata.Select(pair => pair.NameCustom).ToList();
                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    // Apply formatting to header cells
                    var headerCell = worksheet.Cell(1, i + 1);
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                // Get data for each property
                for (int i = 0; i < data.PairDataExcel.Count; i++)
                {
                    var propertyName = data.PairDataExcel[i].NameTable;
                    var propertyData = await GetDataFromDatabase(data.BatchId, propertyName); // Replace with your own method to retrieve data from the database

                    for (int j = 0; j < propertyData.Count; j++)
                    {
                        worksheet.Cell(j + 2, i + 1).Value = propertyData[j];
                    }

                    // Adjust column width to fit the content
                    worksheet.Column(i + 1).AdjustToContents();
                }

                // Save the Excel file to a memory stream
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        //public async Task<byte[]> CreateReportGradeExcelFile(ReportExcelRequest data)
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var namesheet = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.Id == data.BatchId);
        //        var worksheet = workbook.Worksheets.Add(namesheet.Name);

        //        var pairdata = data.PairDataExcel.ToList();
        //        // Add headers
        //        var headers = pairdata.Select(pair => pair.NameCustom).ToList();
        //        for (int i = 0; i < headers.Count; i++)
        //        {
        //            worksheet.Cell(1, i + 1).Value = headers[i];
        //            // Apply formatting to header cells
        //            var headerCell = worksheet.Cell(1, i + 1);
        //            headerCell.Style.Font.Bold = true;
        //            headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
        //            // Adjust column width to fit the content
        //            worksheet.Column(i + 1).AdjustToContents();
        //        }

        //        // Get data for each property
        //        for (int i = 0; i < data.PairDataExcel.Count; i++)
        //        {
        //            var propertyName = data.PairDataExcel[i].NameTable;
        //            var propertyData = await GetDataFromDatabase(data.BatchId, propertyName); // Replace with your own method to retrieve data from the database

        //            for (int j = 0; j < propertyData.Count; j++)
        //            {
        //                worksheet.Cell(j + 2, i + 1).Value = propertyData[j];
        //                // Apply formatting to data cells
        //                var dataCell = worksheet.Cell(j + 2, i + 1);
        //                dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                dataCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //            }
        //        }

        //        // Save the Excel file to a memory stream
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            workbook.SaveAs(memoryStream);
        //            return memoryStream.ToArray();
        //        }
        //    }
        //}


        public async Task<List<string>> GetDataFromDatabase(int batchid, string propertyName)
        {
            var trannee = await _unitOfWork.UserRepository.GetTraineeListByBatch(batchid);
            var propertyValues = new List<string>();

            foreach (var trainee in trannee.ToList())
            {
                var property = trainee.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(trainee);
                    if (value != null)
                    {
                        propertyValues.Add(value.ToString());
                    }
                    else
                    {
                        propertyValues.Add(string.Empty);
                    }
                }
            }
            return propertyValues;
        }
    }
}
