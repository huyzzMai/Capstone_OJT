using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("CriteriaDetail")]
    public class CriteriaDetail
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public int? Point { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? CriteriaId { get; set; }
        [ForeignKey("CriteriaId")]
        public Criteria Criteria { get; set; }

    }
}
