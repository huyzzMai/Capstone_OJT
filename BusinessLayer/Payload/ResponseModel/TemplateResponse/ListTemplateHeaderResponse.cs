using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.TemplateResponse
{
    public class ListTemplateHeaderCriteriaResponse
    {
        public int TeamplateHeaderId { get; set; }

        public string Name { get; set; }

        public double? MaxPoint { get; set; }
    }
}
