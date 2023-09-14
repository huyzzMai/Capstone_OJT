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
        
        public string StartTime { get; set; }
        
        public string EndTime { get; set; }

        public int? TemplateId { get; set; }

    }
}
