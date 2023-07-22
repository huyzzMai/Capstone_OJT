using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CourseResponse
{
    public class CoursePositionResponse
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public bool? IsCompulsory { get; set; }
    }
}
