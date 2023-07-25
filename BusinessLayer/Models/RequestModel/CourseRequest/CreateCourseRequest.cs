using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class CreateCourseRequest
    {
        [Required]
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }        
        public string ImageURL { get; set; }

        public List<CoursePositionRequest> CoursePosition { get; set; }

        public List<CourseSkillRequest> CourseSkills { get; set; }

    }
}
