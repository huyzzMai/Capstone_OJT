﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.UserResponse
{
    public class TraineeResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public string AvatarURL { get; set; }
        public int Gender { get; set; }
        public int? Stauts { get; set; }
    }
}
