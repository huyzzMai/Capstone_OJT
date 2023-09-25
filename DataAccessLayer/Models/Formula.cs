using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("Formula")]
    public class Formula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(700)")]
        public string Calculation { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public int? Status { get; set; }    
    }
}
