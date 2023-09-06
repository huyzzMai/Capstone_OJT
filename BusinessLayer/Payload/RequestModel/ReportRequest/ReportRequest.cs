using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.ReportRequest
{
    public class ReportRequest
    {
        public string url { get; set; }
        public int batchId { get; set; }
    }
}
