using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel
{
    public class PagingRequestModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
