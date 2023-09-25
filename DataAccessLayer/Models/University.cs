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
    [Table("University")]
    public class University
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(700)")]
        public string ImgURL { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        public string UniversityCode { get; set; }  

        [Column(TypeName = "nvarchar(500)")]
        public string Address { get; set; } 

        public int? Status { get; set; }

        public DateTime? JoinDate { get; set; }

        //public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<OJTBatch> OJTBatches { get; set; }

        public virtual ICollection<Template> Templates { get; set; }
    }
}
