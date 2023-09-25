using BusinessLayer.Payload.ResponseModel.Attendanceesponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.ExcelResponse
{
    public class AttendanceUserResponse
    {
        public int userId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? numberOfDateforget { get; set; }

        public List<AttendanceDetail> presentDay { get; set; }     
              
    }
}
