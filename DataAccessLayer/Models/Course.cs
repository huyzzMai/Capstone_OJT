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
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Link { get; set; }

        public DateTime? StartTime { get; set; } 

        public DateTime? EndTime { get; set; }

        public bool? IsCompulsory { get; set; }

        public int? Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
