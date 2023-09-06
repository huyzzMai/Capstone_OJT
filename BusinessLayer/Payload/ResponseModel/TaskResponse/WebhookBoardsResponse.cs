using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.TaskResponse
{
    public class WebhookBoardsResponse
    {
        public string BoardTrelloId { get; set; }
        public string BoardName { get; set; }
        public string BoardURL { get; set; }
    }
}
