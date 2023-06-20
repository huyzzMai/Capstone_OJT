using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("UserCriteria")]
    public class UserCriteria
    {      
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int CriteriaId { get; set; }
        [ForeignKey("CriteriaId")]
        public virtual Criteria Criteria { get; set; }

        public int? Point { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
