﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.UniversityRequest
{
    public class UpdateUniversityRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImgURL { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int? Status { get; set; }
        [Required]
        public DateTime? JoinDate { get; set; }
    }
}