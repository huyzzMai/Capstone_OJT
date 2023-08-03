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
    public class UserDetailResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public int? Gender { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Birthday")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? Birthday { get; set; }
        public int? Status { get; set; }
        public int? Role { get; set; }
        [JsonProperty(PropertyName = "CreatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty(PropertyName = "UpdatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }
    }
}
