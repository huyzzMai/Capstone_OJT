using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class CourseSkillRequest
    {
        public int SkillId { get; set; }

        public int? RecommendedLevel { get; set; }

        public int? AfterwardLevel { get; set; }
    }
}
