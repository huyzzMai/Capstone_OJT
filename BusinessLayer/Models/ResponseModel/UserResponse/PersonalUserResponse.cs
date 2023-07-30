using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.UserResponse
{
    public class PersonalUserResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Gender { get; set; } 
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string RollNumber { get; set; }    
        public string AvatarURL { get; set; }   
        public int Position { get; set; }
    }
}
