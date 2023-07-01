using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.TrainingPlanResponse
{
    public class TrainingPlanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
    }
}
