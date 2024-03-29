﻿using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UserResponse
{
    public class UserDetailResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public int? Gender { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        
        public string Birthday { get; set; }
        public int? Status { get; set; }
        public int? Role { get; set; }
        
        public string CreatedAt { get; set; }
        
        public string UpdatedAt { get; set; }
    }
}
