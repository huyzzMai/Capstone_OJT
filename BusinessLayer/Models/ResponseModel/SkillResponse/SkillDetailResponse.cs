﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.SkillResponse
{
    public class SkillDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Type { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
