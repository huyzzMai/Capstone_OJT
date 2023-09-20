using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("TaskAccomplished")]
    public class TaskAccomplished
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string TrelloTaskId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public DateTimeOffset? AccomplishDate { get; set; }

        public int? Status { get; set; }

        // Relation with User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
