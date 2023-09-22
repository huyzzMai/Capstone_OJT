using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.AttendanceResponse
{
    public class AttendanceByMonthResponse
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public List<AttendanceInMonth> attendanceInMonth { get; set; }

    }

    public class AttendanceInMonth
    {
        public int Day { get; set; }

        public int totalRecords { get; set; }

    }
}
