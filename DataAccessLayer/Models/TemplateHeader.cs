using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("TemplateHeader")]
    public class TemplateHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }    

        public double? TotalPoint { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string MatchedAttribute { get; set; }  

        public bool? IsCriteria { get; set; }

        public int? Order { get; set; } 

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relation with User
        public virtual ICollection<UserCriteria> UserCriterias { get; set; }

        // Relation with Template
        public int TemplateId { get; set; }
        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        // Relation with Formula
        public int? FormulaId { get; set; }
        [ForeignKey("FormulaId")]
        public Formula Formula { get; set; }
    }
}
