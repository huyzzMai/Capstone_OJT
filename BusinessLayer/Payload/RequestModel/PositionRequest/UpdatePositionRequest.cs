using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.PositionRequest
{
    public class UpdatePositionRequest
    {
        public string Name { get; set; }
        public int? Status { get; set; }

    }
}
