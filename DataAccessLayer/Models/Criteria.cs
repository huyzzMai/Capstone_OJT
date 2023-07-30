﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("Criteria")]
    public class Criteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }    

        public int? TotalPoint { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relation with User
        public virtual ICollection<UserCriteria> UserCriterias { get; set; }

        // Relation with University
        public int UniversityId { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }

        // Relation with Template
        //public virtual ICollection<TemplateCriteria> TemplateCriterias { get; set; }
    }
}