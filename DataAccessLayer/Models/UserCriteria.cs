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
        public User User { get; set; }

        public int TemplateHeaderId { get; set; }
        [ForeignKey("TemplateHeaderId")]
        public TemplateHeader TemplateHeader { get; set; }

        public double? Point { get; set; }

        public int? TrainerIdMark { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get;set; }

    }
}
