using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("UserTrainingPlan")]
    public class UserTrainingPlan
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int TrainingPlanId { get; set; }
        [ForeignKey("TrainingPlanId")]
        public virtual TrainingPlan TrainingPlan { get; set; }

        public bool? IsOwner { get; set; }
    }
}
