using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class UpdateCourseRequest
    {
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageURL { get; set; }
        public int? Status { get; set; }
    }
}
