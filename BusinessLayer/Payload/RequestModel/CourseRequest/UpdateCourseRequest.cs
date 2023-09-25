using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.CourseRequest
{
    public class UpdateCourseRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PlatformName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public int? Status { get; set; }
    }
}
