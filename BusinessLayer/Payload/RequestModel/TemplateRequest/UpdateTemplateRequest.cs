using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TemplateRequest
{
    public class UpdateTemplateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string StartCell { get; set; }
        [Required]
        public int? Status { get; set; }
    }
}
