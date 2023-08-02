using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.OjtBatchRequest
{
    public class CreateOjtBatchRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }

        [Required]
        public int? UniversityId { get; set; }
    }
}
