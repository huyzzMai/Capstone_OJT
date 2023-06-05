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
    public class User
    {
        [Key]
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

        public int? Status { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
