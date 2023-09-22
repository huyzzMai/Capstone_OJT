using BusinessLayer.Payload.RequestModel.TrainingPLanRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.TrainingPlanResponse
{
    public class TrainingPlanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public DateTime CreateDate { get; set; }    
        public DateTime UpdateDate { get; set; }

        public int TrainerId { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }    

        public List<TrainingPlanDetailResponse> Details { get; set; }
    }
}
