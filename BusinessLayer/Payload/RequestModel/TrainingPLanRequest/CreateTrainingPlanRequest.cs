using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TrainingPLanRequest
{
    public class CreateTrainingPlanRequest
    {
        [Required]
        public string Name { get; set; }

        public List<CreateTrainingPlanDetailRequest> Details { get; set; }
    }
}
