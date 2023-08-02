using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.OJTBatchResponse
{
    public class OjtBatchDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [JsonProperty(PropertyName = "StartTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? StartTime { get; set; }
        [JsonProperty(PropertyName = "EndTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EndTime { get; set; }

        public bool? IsDeleted { get; set; }
        [JsonProperty(PropertyName = "CreatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty(PropertyName = "UpdatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }

        public int? UniversityId { get; set; }
    }
}
