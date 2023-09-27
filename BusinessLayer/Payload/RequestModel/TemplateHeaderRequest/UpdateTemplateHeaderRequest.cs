using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TemplateHeaderRequest
{
    public class UpdateTemplateHeaderRequest
    {
        [Required]
        public string Name { get; set; }     
        public double? TotalPoint { get; set; }       
        public string MatchedAttribute { get; set; }
        [Required]
        public bool? IsCriteria { get; set; }
        public int FormulaId { get; set; }
    }
}
