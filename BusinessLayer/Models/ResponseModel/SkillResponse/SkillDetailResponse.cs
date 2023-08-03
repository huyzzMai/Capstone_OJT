using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.SkillResponse
{
    public class SkillDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }
        [JsonProperty(PropertyName = "CreatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty(PropertyName = "UpdatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }
    }
}
