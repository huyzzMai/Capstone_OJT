using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("Template")]
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Url { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string StartCell { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UniversityId { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }

        // Relation with Template Header
        public virtual ICollection<TemplateHeader> TemplateHeaders { get; set; }
    }
}
