using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.PositionResponse
{
    public class PositionDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }
    }
}
