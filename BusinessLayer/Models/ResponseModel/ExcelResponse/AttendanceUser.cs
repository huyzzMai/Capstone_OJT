using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.ExcelResponse
{
    public class AttendanceUser
    {
        public int MaNV { get; set; }
        public DateTime date { get; set; }
        public double totalTime { get; set; }
    }
}
