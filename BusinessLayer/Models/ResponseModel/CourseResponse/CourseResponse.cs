using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CourseResponse
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageURL { get; set; }
        public int TotalEnrollment { get; set; }
        public int TotalActiveEnrollment { get; set; }
        public List<CoursePositionResponse>? coursePositions { get; set; }
        public List<CourseSkillResponse>? courseSkills { get; set; }

    }
}
