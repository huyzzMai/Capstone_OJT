using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.FormulaRequest
{
    public class CreateFormulaRequest
    {
        public string Calculation { get; set; }

        public string Name { get; set; }
    }
}
