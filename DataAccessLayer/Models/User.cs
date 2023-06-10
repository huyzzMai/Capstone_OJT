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
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string RollNumber { get; set; }

        public int? Gender { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Birthday { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string AvatarUrl { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Position { get; set; }

        public int? Role { get; set; }

        public int? Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relation with OJTBatch 
        public int? OJTBatchId { get; set; }
        [ForeignKey("OJTBatchId")]
        public OJTBatch OJTBatch { get; set; }

        // Relation with Training Plan 
        public int? TrainingPlanId { get; set; }
        [ForeignKey("TrainingPlanId")]
        public TrainingPlan TrainingPlan { get; set; }

        // Relation of Trainer and Trainee
        public int? UserReferenceId { get; set; }
        [ForeignKey("UserReferenceId")]
        public User Trainer { get; set; }
        public virtual ICollection<User> Trainees { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }


        public virtual ICollection<Criteria> Criterias { get; set; }

    }
}
