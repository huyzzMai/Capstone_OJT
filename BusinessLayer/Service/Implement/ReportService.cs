using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using ClosedXML.Excel;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
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
        public static List<string> GetPropertyDataforUser(string propertyName, List<User> userList)
        {
            var query = userList.OrderBy(c => c.Id).AsQueryable();


            var propertyInfo = typeof(User).GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, $"Property '{propertyName}' not found in User class.");
            }

            var values = query.Select(u => (string)propertyInfo.GetValue(u)).ToList();
            return values;
        }
        public static List<string> GetPropertyDataforCriteria(string propertyName, List<UserCriteria> userList)
        {
            var query = userList.OrderBy(c => c.UserId).AsQueryable();
            var propertyInfo = typeof(UserCriteria).GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, $"Property '{propertyName}' not found in User class.");
            }

            var values = query.Select(u => propertyInfo.GetValue(u)).ToList();

            List<string> stringValues = new List<string>();
            foreach (var value in values)
            {
                string stringValue = value?.ToString() ?? "";
                stringValues.Add(stringValue);
            }

            return stringValues;
        }




        public async Task<List<List<string>>> GenerateData(Template headers)
        {
            List<List<string>> dataMap = new List<List<string>>();
            var th = headers.TemplateHeaders.Where(c => c.Status == CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE).OrderBy(c => c.Order);
            foreach (var h in th)
            {
                List<string> list = new List<string>();
                if (h.IsCriteria == true)
                {
                    list = GetPropertyDataforCriteria(h.MatchedAttribute, h.UserCriterias.ToList());
                    dataMap.Add(list);
                }
                else
                {
                    var users = await _unitOfWork.UserRepository.Get(c => c.Status == CommonEnums.USER_STATUS.ACTIVE && c.OJTBatch.UniversityId == headers.UniversityId, "OJTBatch");
                    list = GetPropertyDataforUser(h.MatchedAttribute, users.ToList());
                    dataMap.Add(list);
                }
            }
            return dataMap;
        }

        public static int ColIndexToNumber(string colIndex)
        {
            int col = 0;
            int mul = 1;

            for (int i = colIndex.Length - 1; i >= 0; i--)
            {
                col += (colIndex[i] - 'A' + 1) * mul;
                mul *= 26;
            }

            return col;
        }

        public static (int row, int col) GetRowAndColumnFromCellIndex(string cellIndex)
        {
            if (Regex.IsMatch(cellIndex, "^[A-Z]+[0-9]+$", RegexOptions.IgnoreCase))
            {
                string colString = new string(cellIndex.TakeWhile(char.IsLetter).ToArray());
                string rowString = new string(cellIndex.SkipWhile(char.IsLetter).ToArray());
                int col = ColIndexToNumber(colString);
                int row = int.Parse(rowString);
                return (row, col);
            }
            else
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Invalid cell index format. Expected format: letter followed by number (e.g., 'G14').");
            }
        }

        public byte[] UpdateExcelFile(int startRow, int startCol, byte[] excelData, List<List<string>> dataMap)
        {
            using (var package = new ExcelPackage(new MemoryStream(excelData)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int columnCount = worksheet.Dimension.Columns;
                if (rowCount < startRow || columnCount < startCol)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Start index is over colum or row");
                }
                int currentCol = startCol;
                foreach (var rowData in dataMap)
                {

                    int currentRow = startRow;
                    foreach (string data in rowData)
                    {
                        var cell = worksheet.Cells[currentRow, currentCol];
                        if (cell.Value == null)
                        {
                            cell.Value = data;
                        }
                        else
                        {
                            throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "There is data at blank need insert data");
                        }
                        currentRow++;
                    }
                    currentCol++;
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }




        public async Task<byte[]> ExportReportExcelFileFromUniversity(byte[] excelStream, int templateid)
        {
            try
            {
                var template = await _unitOfWork.TemplateRepository.GetFirst(c => c.Status == CommonEnums.TEMPLATE_STATUS.ACTIVE && c.Id == templateid, "TemplateHeaders");
                if (template == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template not found");
                }
                var data = await GenerateData(template);
                (int row, int col) = GetRowAndColumnFromCellIndex(template.StartCell);
                var updatedExcelData = UpdateExcelFile(row, col, excelStream, data);
                return updatedExcelData;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
