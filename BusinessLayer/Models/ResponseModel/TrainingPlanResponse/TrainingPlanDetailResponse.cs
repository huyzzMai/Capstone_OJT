using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.TrainingPlanResponse
{
    public class TrainingPlanDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public bool? IsEvaluativeTask { get; set; }

        public int? Status { get; set; }    

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
