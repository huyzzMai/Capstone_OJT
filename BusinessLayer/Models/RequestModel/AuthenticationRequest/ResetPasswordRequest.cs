using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.AuthenticationRequest
{
    public class ResetPasswordRequest
    {
        public string ResetCode { get; set; }    
        public string NewPassword { get; set; }
    }
}
