using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.AttendanceResponse
{
    public class AttendanceByDateResponse
    {
        public string Day { get; set; }

        public List<AttendanceUser> attendanceUsers { get; set; }
    }
    public class AttendanceUser
    {
        public int UserId { get; set; }
      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string RollNumber { get; set; }

        public string AvatarURL { get; set; }

        public TimeSpan? totalWorkingHours { get; set; }
    }
}
