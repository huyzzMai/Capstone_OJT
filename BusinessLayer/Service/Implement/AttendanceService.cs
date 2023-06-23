using BusinessLayer.Models.ResponseModel.ExcelResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Interface;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AttendanceUser>> ProcessAttendanceFile(string filePath)
        {
            var data = new List<AttendanceUser>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Xuất lưới"];

                if (worksheet == null)
                    throw new ArgumentException("Worksheet not found.");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 5; row <= rowCount; row++)
                {
                    AttendanceUser model = new AttendanceUser();
                    model.MaNV = int.Parse(worksheet.Cells[row, 3].Value?.ToString());
                    model.date = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 1].Value?.ToString()));
                    model.totalTime = double.Parse(worksheet.Cells[row, 11].Value?.ToString());

                    data.Add(model);
                }
            }

            // Delete the temporary file
            File.Delete(filePath);

            return data;
        }
        public async Task<string> SaveTempFile(IFormFile file)
        {
            var tempFileName = Path.GetTempFileName(); // Generate a unique temporary file name
            var filePath = Path.ChangeExtension(tempFileName, Path.GetExtension(file.FileName)); // Use the original file extension

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Save the file to the server
            }

            return filePath;
        }


    }
}
