﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.SkillResponse
{
    public class SkillResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }
    }
}
