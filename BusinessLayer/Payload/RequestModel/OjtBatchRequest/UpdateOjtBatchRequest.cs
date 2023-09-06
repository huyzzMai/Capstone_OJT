using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.OjtBatchRequest
{
    public class UpdateOjtBatchRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public int? TemplateId { get; set; }
        [Required]
        public int? UniversityId { get; set; }
        [Required]
        public bool? IsDeleted { get; set; }
    }
}
