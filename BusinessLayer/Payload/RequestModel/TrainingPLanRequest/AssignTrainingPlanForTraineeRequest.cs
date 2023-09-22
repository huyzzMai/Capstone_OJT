using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TrainingPLanRequest
{
    public class AssignTrainingPlanForTraineeRequest
    {
        public class AssignTraineeIdRequest
        {
            public int TraineeId { get; set; }
        }

        [Required]
        public int TrainingPlanId { get; set; }

        public List<AssignTraineeIdRequest> Trainees { get; set; }
    }
}
