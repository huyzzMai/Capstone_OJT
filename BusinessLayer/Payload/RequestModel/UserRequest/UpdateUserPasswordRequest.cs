using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.UserRequest
{
    public class UpdateUserPasswordRequest
    {
        public string OldPassword { get; set; }

        public string NewPassord { get; set; }
    }
}
