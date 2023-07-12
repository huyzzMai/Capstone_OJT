using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel
{
    public class PagingRequestModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
