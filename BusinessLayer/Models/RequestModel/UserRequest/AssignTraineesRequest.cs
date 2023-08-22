using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.UserRequest
{
    public class AssignTraineesRequest
    {
        public class TraineeIdRequest
        {
            public int TraineeId { get; set; }  
        }

        [Required]
        public int TrainerId { get; set; }

        public List<TraineeIdRequest> Trainees { get; set; }
    }
}
