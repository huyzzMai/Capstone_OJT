using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UserResponse
{
    public class PersonalUserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Gender { get; set; } 
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string RollNumber { get; set; }    
        public string AvatarURL { get; set; }   
        public string PositionName { get; set; }
        public string StudentCode { get; set; }

        public class PersonalTrainerResponse
        {
            public string TrainerName { get; set; }
            public string TrainerEmail { get; set; }
            public string TrainerPhoneNumber { get; set; }
            public string AvatarURL { get; set; }
        }
        public PersonalTrainerResponse TrainerResponse { get; set; }    

        public class PersonalSkillResponse
        {
            public string Name { get; set; }
            //public int Type { get; set; }
            public int CurrentLevel { get; set; }   
        }
        public List<PersonalSkillResponse> Skills { get; set; }
    }
}
