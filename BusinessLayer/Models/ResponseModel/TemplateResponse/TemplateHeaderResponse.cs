using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.TemplateResponse
{
    public class TemplateHeaderResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? TotalPoint { get; set; }

        public int? FormulaId { get; set; }

        public string MatchedAttribute { get; set; }


        public bool? IsCriteria { get; set; }

        public int? Order { get; set; }

        public int? Status { get; set; }

    }
}
