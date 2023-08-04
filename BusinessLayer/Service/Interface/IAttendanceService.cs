using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.ExcelResponse;
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

        //Task<string> SaveTempFile(IFormFile file);
    }
}
