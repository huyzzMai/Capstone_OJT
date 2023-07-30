using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CourseRequest
{
    public class CourseSkillRequest
    {
        [Required]
        public int SkillId { get; set; }
        [Required]
        public int? RecommendedLevel { get; set; }
        [Required]
        public int? AfterwardLevel { get; set; }
    }
}
