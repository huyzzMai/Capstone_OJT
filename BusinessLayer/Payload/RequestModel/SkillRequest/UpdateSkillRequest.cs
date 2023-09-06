using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.SkillRequest
{
    public class UpdateSkillRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Status { get; set; }
    }
}
