using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.Attendanceesponse
{
    public class AttendanceDetail
    {
        public string day { get; set; }
        public TimeSpan? totalWorkingTime { get; set; }
    }
}
