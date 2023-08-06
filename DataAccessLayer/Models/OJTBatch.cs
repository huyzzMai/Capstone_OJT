using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("OJTBatch")]
    public class OJTBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UniversityId { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }

        public int? TemplateId { get; set; }
        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        public virtual ICollection<User> Trainees { get; set; }

        public virtual ICollection<CourseBatch> CourseBatches { get; set; }
    }
}
