using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.TaskResponse
{
    public class TaskAccomplishedResponse
    {
        public int Id { get; set; }

        public string TrelloTaskId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public DateTimeOffset? FinishTime { get; set; }

        public int? ProcessStatus { get; set; }
    }
}
