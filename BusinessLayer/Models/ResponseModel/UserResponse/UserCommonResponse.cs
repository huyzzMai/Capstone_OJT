using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.UserResponse
{
    public class UserCommonResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string RollNumber { get; set; }

        public int? Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        
        public string Birthday { get; set; }

        public string AvatarURL { get; set; }

        public int? Position { get; set; }  

        public int? Role { get; set; }

        public string TrelloId { get; set; }
      
        public string CreatedAt { get; set; }
       
        public string UpdatedAt { get; set; }
    }
}
