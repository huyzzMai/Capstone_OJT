using BusinessLayer.Utilities;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CourseResponse
{
    public class CourseDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PlatformName { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }


        public string ImageURL { get; set; }

        public int? Status { get; set; }
        [JsonProperty(PropertyName = "CreatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty(PropertyName = "UpdatedAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<CoursePositionResponse>? CoursePositions { get; set; }

        public virtual ICollection<CourseSkillResponse>? CourseSkills { get; set; }
    }
}
