using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.AttendanceResponse;
using BusinessLayer.Payload.ResponseModel.ExcelResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IAttendanceService
    {
        Task<List<AttendanceUserResponse>> ProcessAttendanceFile(IFormFile file);

        Task<AttendanceByMonthResponse> GetAttendanceByMonth(int month,int year);

        Task<AttendanceByDateResponse> GetAttendanceByDate(DateTime date);

    }
}
