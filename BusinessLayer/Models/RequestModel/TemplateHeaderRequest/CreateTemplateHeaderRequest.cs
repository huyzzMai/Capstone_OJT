﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.TemplateHeaderRequest
{
    public class CreateTemplateHeaderRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double? TotalPoint { get; set; }
        [Required]
        public string MatchedAttribute { get; set; }
        [Required]
        public bool? IsCriteria { get; set; }
        [Required]
        public int? Order { get; set; }

    }
}
