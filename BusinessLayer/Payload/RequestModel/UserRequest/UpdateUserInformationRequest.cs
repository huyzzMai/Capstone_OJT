﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.UserRequest
{
    public class UpdateUserInformationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public int? Gender { get; set; }    
        public string Address { get; set; }
        public string AvatarURL { get; set; }
    }
}
