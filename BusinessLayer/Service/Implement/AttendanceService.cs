using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.Attendanceesponse;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel.ExcelResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static DataAccessLayer.Commons.CommonEnums;

namespace BusinessLayer.Service.Implement
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool HasDuplicateItems(List<Attendance> list1, List<Attendance> list2)
        {
            var uniqueItemsList1 = list1.Select(a => new { a.UserId, a.PresentDate }).ToList();
            foreach (var item in list2)
            {
               
                if (uniqueItemsList1.Any(x => x.UserId == item.UserId && IsSameDate(x.PresentDate, item.PresentDate)))
                {
                    return true;
                }
            }

            return false; 
        }     
        private bool IsSameDate(DateTime? date1, DateTime? date2)
        {
            if (date1 == null && date2 == null)
                return true;
            if (date1 == null || date2 == null)
                return false;
            return date1.Value.Date == date2.Value.Date;
        }
        public List<AttendanceUserResponse> GetListResponseAttendUser(List<Attendance> listattend, List<User> listUser)
        {
            var usersInAttendance = listUser
            .Where(user => listattend.Any(attendance => attendance.UserId == user.Id))
            .ToList();
            var lisresponse = new List<AttendanceUserResponse>();
            foreach (var user in usersInAttendance)
            {               
                var attendanceresponse = new AttendanceUserResponse()
                {
                    userId = user.Id,
                    presentDay = listattend.Where(c => c.UserId == user.Id).Select(c => new AttendanceDetail
                    {
                        day = DateTimeService.ConvertToDateString(c.PresentDate),
                        totalWorkingTime = c.TotalTime
                    }).ToList()
                };
                lisresponse.Add(attendanceresponse);
            }
            return lisresponse;
        }
        public async Task<List<Attendance>> GetAttendance(IFormFile file)
        {
            var attendances = new List<Attendance>();
            using (var stream = file.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                if (worksheet == null)
                    throw new ArgumentException("Worksheet not found.");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 5; row <= rowCount; row++)
                {
                    var MaNV = worksheet.Cells[row, 3].Value?.ToString();
                    var user = await _unitOfWork.UserRepository.GetFirst(c => c.Status != CommonEnums.USER_STATUS.DELETED && c.RollNumber == MaNV);
                    if(user == null)
                    {
                        continue;
                    }
                    var model = new Attendance()
                    {
                        UserId = user.Id,
                        PresentDate = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 1].Value?.ToString())),
                        TotalTime = DateTimeService.ConvertToTimeSpan(double.Parse(worksheet.Cells[row, 11].Value?.ToString()))
                    };                   
                    attendances.Add(model);
                }
                var listattendDb = await _unitOfWork.AttendanceRepository.Get();
                if (HasDuplicateItems(attendances, listattendDb.ToList()))
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT,"Duplicate attendences import fail!");
                }
                foreach (var attendance in attendances)
                {
                    await _unitOfWork.AttendanceRepository.Add(attendance);
                }                   
            }
            return attendances;
        }
        public async Task<List<AttendanceUserResponse>> ProcessAttendanceFile(IFormFile file)
        {
            try
            {
                var data = new List<AttendanceUserResponse>();
                var attendances=await GetAttendance(file);
                var userlist = await _unitOfWork.UserRepository.Get(c => c.Status != CommonEnums.USER_STATUS.DELETED);
                data = GetListResponseAttendUser(attendances, userlist.ToList());
                return data;
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
        //public async Task<string> SaveTempFile(IFormFile file)
        //{
        //    try
        //    {
        //        var tempFileName = Path.GetTempFileName();
        //        var filePath = Path.ChangeExtension(tempFileName, Path.GetExtension(file.FileName));
        //        if(filePath==null)
        //        {
        //            throw new  ApiException(CommonEnums.CLIENT_ERROR.CONFLICT,"Set file path fail");
        //        }
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //        return filePath;
        //    }
        //    catch (ApiException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }            
        //}


    }
}
