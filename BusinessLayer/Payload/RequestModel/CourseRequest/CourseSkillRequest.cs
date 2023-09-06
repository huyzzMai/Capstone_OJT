using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.CourseRequest
{
    public class CourseSkillRequest
    {
        [Required]
        public int SkillId { get; set; }
        [Required]
        [Range(0,5)]
        public int? RecommendedLevel { get; set; }
        [Required]
        [Range(0, 5)]
        public int? AfterwardLevel { get; set; }
    }
}
