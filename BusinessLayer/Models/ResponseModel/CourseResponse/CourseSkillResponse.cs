using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CourseResponse
{
    public class CourseSkillResponse
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; }

        public int? RecommendedLevel { get; set; }

        public int? AfterwardLevel { get; set; }
    }
}
