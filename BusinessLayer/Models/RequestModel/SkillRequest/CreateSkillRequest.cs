using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.SkillRequest
{
    public class CreateSkillRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Status { get; set; }
    }
}
