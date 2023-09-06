using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.CourseRequest
{
    public class CoursePositionRequest
    {
        [Required]
        public int PositionId { get; set; }
        [Required]
        public bool? IsCompulsory { get; set; }
    }
}
