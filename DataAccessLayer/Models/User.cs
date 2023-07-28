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

        public DateTime? Birthday { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string AvatarURL { get; set; }

        public int? Position { get; set; }

        public int? Role { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ResetPassordCode { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string TrelloId { get; set; }

        //public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relation with OJTBatch 
        public int? OJTBatchId { get; set; }
        [ForeignKey("OJTBatchId")]
        public OJTBatch OJTBatch { get; set; }

        // Relation with Training Plan
        public virtual ICollection<UserTrainingPlan> UserTrainingPlans { get; set; }

        // Relation of Trainer and Trainee
        public int? UserReferenceId { get; set; }
        [ForeignKey("UserReferenceId")]
        public User Trainer { get; set; }
        public virtual ICollection<User> Trainees { get; set; }

        // Relation with Attendance
        public virtual ICollection<Attendance> Attendances { get; set; }

        // Relation with Template Header
        public virtual ICollection<UserCriteria> UserCriterias { get; set; }

        // Relation with Skill
        public virtual ICollection<UserSkill> UserSkills { get; set; }

        // Relation with Certificate
        public virtual ICollection<Certificate> Certificates { get; set; }


        // Relation with Notification
        public virtual ICollection<Notification> Notifications { get; set; }

        // Relation with Task
        public virtual ICollection<TaskAccomplished> TaskAccomplished { get; set; }
    }
}
