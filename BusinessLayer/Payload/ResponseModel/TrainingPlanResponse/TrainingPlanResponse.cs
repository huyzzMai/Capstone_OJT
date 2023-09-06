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

        public List<TrainingPlanDetailResponse> Details { get; set; }
    }
}
