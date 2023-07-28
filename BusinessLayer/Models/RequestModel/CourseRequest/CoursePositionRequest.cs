using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class CoursePositionRequest
    {
        [Required]
        public int Position { get; set; }
        [Required]
        public bool? IsCompulsory { get; set; }
    }
}
