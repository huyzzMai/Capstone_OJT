using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TrainingPLanRequest
{
    public class UpdateTrainingPlanRequest
    {
        public string Name { get; set; }

        public List<UpdateTrainingPlanDetailRequest> Details { get; set; }

        public class UpdateTrainingPlanDetailRequest
        {
            public int? Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public DateTime? StartTime { get; set; }
            [Required]
            public DateTime? EndTime { get; set; }
            public int? Status { get; set; }    
        }
    }
}
