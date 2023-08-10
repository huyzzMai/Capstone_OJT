using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CriteriaRequest
{
    public class UpdateCriteriaRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public List<UpdateUserCriteria> UserCriterias { get; set; }

    }
}
