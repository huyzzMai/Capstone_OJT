using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("UserSkill")]
    public class UserSkill
    {
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public Skill Skill { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Course User { get; set; }

        public int? Level { get; set; }
    }
}
