using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class CoursePositionRequest
    {
        public string Position { get; set; }
        public bool? IsCompulsory { get; set; }
    }
}
