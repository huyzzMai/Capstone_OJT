using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UserResponse
{
    public class TrainerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarURL { get; set; }
        public int Gender { get; set; } 
        public string RollNumber { get; set; }  
        public string PositionName { get; set;}
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public int? Status { get; set; }    
    }
}
