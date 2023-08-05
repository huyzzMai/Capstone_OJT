using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string PlatformName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Link { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string ImageURL { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relation with Position
        public virtual ICollection<CoursePosition> CoursePositions { get; set; }

        // Relation with Skill
        public virtual ICollection<CourseSkill> CourseSkills { get; set; }

        // Relation with User by Certificate
        public virtual ICollection<Certificate> Certificates { get; set; }

        // Relation with Batch 
        public virtual ICollection<CourseBatch> CourseBatches { get; set; } 
    }
}
