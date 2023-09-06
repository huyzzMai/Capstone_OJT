using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.CourseResponse
{
    public class CoursePositionResponse
    {
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public bool? IsCompulsory { get; set; }
    }
}
