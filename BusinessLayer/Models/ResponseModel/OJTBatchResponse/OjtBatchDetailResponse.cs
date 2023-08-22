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
        
        public string StartTime { get; set; }
        
        public string EndTime { get; set; }

        public int? TemplateId { get; set; }

        public bool? IsDeleted { get; set; }
        
        public string CreatedAt { get; set; }
        
        public string UpdatedAt { get; set; }

        public int? UniversityId { get; set; }
    }
}
