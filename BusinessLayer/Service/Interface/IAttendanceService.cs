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
        Task<IEnumerable<AttendanceUser>> ProcessAttendanceFile(string filePath);

        Task<string> SaveTempFile(IFormFile file);
    }
}
