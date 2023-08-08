using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.UserRequest
{
    public class CreateUserRequest
    {
        [Required]
        public int Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Gender { get; set; }
        public string RollNumber { get; set; }
        public string AvatarUrl { get; set; }
        public int? Position { get; set; }
        public string TrelloId { get; set; }
        public int? BatchId { get; set; }    

        public class CreateUserSkillRequest
        {
            public int SkillId { get; set; }
            public int InitLevel { get; set; }
        }
        public List<CreateUserSkillRequest> CreateSkills { get; set; }
    }
}
