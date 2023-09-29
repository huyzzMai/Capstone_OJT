using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.CriteriaRequest
{
    public class UpdateUserCriteria
    {
        [Required]
        public int TemplateHeaderId { get; set; }
        [Required]
        public double Point { get; set; }
    }
}
