using BusinessLayer.Models.ResponseModel.Attendanceesponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.ExcelResponse
{
    public class AttendanceUserResponse
    {
        public int userId { get; set; }

        public List<AttendanceDetail> presentDay { get; set; }       
    }
}
