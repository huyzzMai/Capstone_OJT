using System;
using System.Collections.Generic;
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

        public int TrainerId { get; set; }

        public List<TraineeIdRequest> Trainees { get; set; }
    }
}
