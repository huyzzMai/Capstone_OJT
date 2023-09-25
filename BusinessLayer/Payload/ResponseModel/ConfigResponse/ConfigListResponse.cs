using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.ConfigResponse
{
    public class ConfigListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
