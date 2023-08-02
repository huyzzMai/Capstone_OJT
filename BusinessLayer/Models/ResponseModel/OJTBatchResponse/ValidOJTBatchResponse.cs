using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessLayer.Models.ResponseModel.OJTBatchResponse
{
    public class ValidOJTBatchResponse
    {
        public int Id { get; set; }      
        public string Name { get; set; }
        [JsonProperty(PropertyName = "StartTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? StartTime { get; set; }
        [JsonProperty(PropertyName = "EndTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EndTime { get; set; }

    }
}
