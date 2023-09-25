using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.Attendanceesponse;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.ExcelResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
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
using BusinessLayer.Payload.ResponseModel.AttendanceResponse;
using DocumentFormat.OpenXml.Bibliography;

namespace BusinessLayer.Service.Implement
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool HasDuplicateItems(Attendance attendance, List<Attendance> list2)
        {         
           
               
                if (list2.Any(x => x.UserId == attendance.UserId && IsSameDate(x.PresentDate, attendance.PresentDate)))
                {
                    return true;
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    presentDay = listattend.Where(c => c.UserId == user.Id).Select(c => new AttendanceDetail
                    {
                        day = DateTimeService.ConvertToDateString(c.PresentDate),
                        totalWorkingTime = c.TotalTime
                    }).ToList(),
                    numberOfDateforget= listattend.Where(c => c.UserId == user.Id && c.TotalTime.Value.Hours <= 0 || c.TotalTime==null).Count()
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
                    var totaltime = double.Parse(worksheet.Cells[row, 11].Value?.ToString());
                    var model = new Attendance()
                    {
                        UserId = user.Id,
                        PresentDate = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 1].Value?.ToString())),
                        TotalTime = DateTimeService.ConvertToTimeSpan(totaltime)
                    };                   
                    attendances.Add(model);
                }
                var listattendDb = await _unitOfWork.AttendanceRepository.Get();
                
                foreach (var attendance in attendances)
                {
                    if (HasDuplicateItems(attendance, listattendDb.ToList()))
                    {
                        continue;
                    }
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
         
        public async Task<AttendanceByMonthResponse> GetAttendanceByMonth(int month, int year)
        {
            try
            {
                var attendance= await _unitOfWork.AttendanceRepository.Get();
                var attendancebymonth = attendance.ToList().Where(c=>c.PresentDate.Value.Month==month && c.PresentDate.Value.Year==year);
                var uniqueAttendance = attendancebymonth.DistinctBy(a => a.PresentDate).ToList();

                if (!uniqueAttendance.Any())
                {
                    return null;
                }
                var attendanceResponse = new AttendanceByMonthResponse()
                {
                    Month = month,
                    Year = year,
                    attendanceInMonth= uniqueAttendance.ToList().OrderBy(c => c.PresentDate.Value.Date).Select(a=>
                    new AttendanceInMonth()
                    {
                        Day= a.PresentDate.Value.Day,
                        totalRecords = attendance.Count(c => c.PresentDate.HasValue 
                        && c.PresentDate.Value.Day == a.PresentDate.Value.Day 
                        && c.PresentDate.Value.Month == month 
                        && c.PresentDate.Value.Year == year)
                    }).ToList()
                };
                return attendanceResponse;
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

        public async Task<AttendanceByDateResponse> GetAttendanceByDate(DateTime date)
        {
            try
            {
                var attendance = await _unitOfWork.AttendanceRepository.Get(expression:null,"User");
                var attendancebyDate = attendance
                    .Where(a => a.PresentDate.HasValue &&
                                a.PresentDate.Value.Date == date.Date)
                    .ToList();
                if (!attendancebyDate.Any())
                {
                   return null;
                }
                var attendanceResponse = new AttendanceByDateResponse()
                {
                    Day=DateTimeService.ConvertToDateString(date.Date),                   
                    attendanceUsers = attendancebyDate.ToList().OrderBy(c => c.UserId).Select(a =>
                    new AttendanceUser()
                    {
                        UserId = a.UserId,
                        FirstName = a.User.FirstName, 
                        LastName = a.User.LastName,
                        Email = a.User.Email,
                        RollNumber = a.User.RollNumber,
                        AvatarURL = a.User.AvatarURL,
                        totalWorkingHours = a.TotalTime

                    }).ToList()
                };
                return attendanceResponse;
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
