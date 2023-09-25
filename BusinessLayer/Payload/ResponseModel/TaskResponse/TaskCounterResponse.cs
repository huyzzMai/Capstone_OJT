using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.TaskResponse
{
    public class TaskCounterResponse
    {
        public int TotalTask { get; set; }

        public int TaskComplete { get; set; }

        public int TaskFail { get; set; }

        public int TaskOverdue { get; set; }
    }
}
