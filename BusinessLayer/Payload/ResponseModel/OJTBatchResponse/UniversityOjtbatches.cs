﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.OJTBatchResponse
{
    public class UniversityOjtbatches
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string TemplateName { get; set; }
    }
}
