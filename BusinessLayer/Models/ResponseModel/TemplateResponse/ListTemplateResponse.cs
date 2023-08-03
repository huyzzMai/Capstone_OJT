﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.TemplateResponse
{
    public class ListTemplateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }

        public string StartCell { get; set; }

        public int? Status { get; set; }

        public int? UniversityId { get; set; }

    }
}
