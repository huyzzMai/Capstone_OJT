using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.ChartResponse
{
    public class TopTaskTraineeResponse
    {
        public string TraineeName { get; set; } 
        public int TotalApprovedTask { get; set; }
    }
}
