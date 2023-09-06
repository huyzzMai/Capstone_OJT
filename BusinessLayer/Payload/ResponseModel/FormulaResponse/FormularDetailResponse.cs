using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.FormulaResponse
{
    public class FormularDetailResponse
    {
        public int Id { get; set; }

        public string Calculation { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }
    }
}
