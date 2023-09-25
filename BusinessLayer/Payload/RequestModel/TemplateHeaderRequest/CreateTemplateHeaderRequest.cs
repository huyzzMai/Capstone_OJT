using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.TemplateHeaderRequest
{
    public class CreateTemplateHeaderRequest
    {
        [Required]
        public string Name { get; set; }
        public double? TotalPoint { get; set; }
        [Required]
        public string MatchedAttribute { get; set; }
        [Required]
        public bool? IsCriteria { get; set; }
        public int? FormulaId { get; set; }
        [Required]
        public int? Order { get; set; }

    }
}
