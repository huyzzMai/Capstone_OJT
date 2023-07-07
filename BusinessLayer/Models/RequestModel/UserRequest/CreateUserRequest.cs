﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.UserRequest
{
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }   
        public string Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public string RollNumber { get; set; }
        public string AvatarUrl { get; set; }   
        public int Role { get; set; }
    }
}