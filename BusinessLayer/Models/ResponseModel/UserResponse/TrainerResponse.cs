using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.UserResponse
{
    public class TrainerResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarURL { get; set; }
        public int Gender { get; set; } 
        public int Position { get; set;}
        public int? Status { get; set; }    
    }
}
