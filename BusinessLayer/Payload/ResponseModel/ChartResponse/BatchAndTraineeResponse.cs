using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.ChartResponse
{
    public class BatchAndTraineeResponse
    {
       public List<int> NumberOfOjtBatches { get; set; } = new List<int>();
       public List<int> NumberofTrainees { get; set; } = new List<int>();
    }
}
