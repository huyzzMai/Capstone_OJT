using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using ClosedXML;
using ClosedXML.Excel;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
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
        static List<string> GetIndexList(int count)
        {
            List<string> indexList = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                indexList.Add(i.ToString());
            }

            return indexList;
        }
        public static List<string> GetPropertyDataforUser(string propertyName, List<User> userList)
        {
            var query = userList.OrderBy(c => c.Id).AsQueryable();
            if(propertyName=="FullName")
            {
                var values = query.Select(u => u.LastName+" "+u.FirstName).ToList();
                return values;
            } else
            {
                var propertyInfo = typeof(User).GetProperty(propertyName);

                if (propertyInfo == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, $"Property '{propertyName}' not found in User class.");
                }
                var values = query.Select(u => (string)propertyInfo.GetValue(u)).ToList();
                return values;
            }         
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
        public List<string> GetTotal(List<User> list)
        {
            List<string> totalist = new List<string>();
            foreach (var user in list)
            {
                var total=user.UserCriterias.Sum(c => c.Point);
                totalist.Add(total.ToString());
            }
            return totalist;
        }
        public async Task<List<List<string>>> GenerateData(Template headers,OJTBatch ojt)
        {
            List<List<string>> dataMap = new List<List<string>>();
            var th = headers.TemplateHeaders.Where(c => c.Status == CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE).OrderBy(c => c.Order);
            foreach (var h in th)
            {
                List<string> list = new List<string>();
                if(string.IsNullOrEmpty(h.MatchedAttribute))
                {
                    foreach(var user in ojt.Trainees)
                    {
                        list.Add("");
                    }
                    dataMap.Add(list);
                    continue;
                } else if (h.MatchedAttribute == "Total")
                {
                    List<string> totallist = GetTotal(ojt.Trainees.ToList());
                    dataMap.Add(totallist);
                    continue;

                }
                else if (h.MatchedAttribute == "STT")
                {
                    List<string> indexList = GetIndexList(ojt.Trainees.Count());
                    dataMap.Add(indexList);
                    continue;

                }
                if (h.IsCriteria == true)
                {                   
                        h.UserCriterias = h.UserCriterias.Where(c => ojt.Trainees.Any(o => o.Id == c.UserId)).ToList();
                        list = GetPropertyDataforCriteria(h.MatchedAttribute, h.UserCriterias.ToList());
                        dataMap.Add(list);                    
                }
                else
                {                   
                        var users = await _unitOfWork.UserRepository.Get(c => c.Status == CommonEnums.USER_STATUS.ACTIVE && c.OJTBatch.UniversityId == headers.UniversityId, "OJTBatch");
                        list = GetPropertyDataforUser(h.MatchedAttribute, ojt.Trainees.ToList());
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
                var countlineinsert = dataMap[0].Count();
                for (int i = 1; i <= countlineinsert; i++)
                {
                    var placeindex = startRow + i;
                    worksheet.Cells[startRow, 1, rowCount, columnCount].Copy(worksheet.Cells[placeindex, 1, rowCount, columnCount]);

                }
                foreach (var rowData in dataMap)
                {

                    int currentRow = startRow;
                    foreach (string data in rowData)
                    {
                        var cell = worksheet.Cells[currentRow, currentCol];
                        
                            cell.Value = data;                    
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




        public async Task<byte[]> ExportReportExcelFileFromUniversity(byte[] excelStream, int  batchId)
        {
            try
            {
                var ojtbatch = await _unitOfWork.OJTBatchRepository.GetFirst(c=>c.Id==batchId);              
                if (ojtbatch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Ojt batch not found");
                }
                var trainees = await _unitOfWork.UserRepository.Get(c => c.OJTBatchId == batchId && c.Status == CommonEnums.USER_STATUS.ACTIVE);
                ojtbatch.Trainees = trainees.ToList();
                if (!trainees.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Ojt batch not found any traniees");
                }
                var template = await _unitOfWork.TemplateRepository.GetFirst(c => c.Status == CommonEnums.TEMPLATE_STATUS.ACTIVE && c.Id == ojtbatch.TemplateId, "TemplateHeaders");
                if (template == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template not found");
                }
                var data = await GenerateData(template, ojtbatch);
                if(!data.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Date not found");
                }
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
