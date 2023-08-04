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
    public class UserListResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string AvatarURL { get; set; }

        public int? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public int? OJTBatchId { get; set; }

        public string Birthday { get; set; }
        public int? Status { get; set; }
        public int? Role { get; set; }

       
    }
}
