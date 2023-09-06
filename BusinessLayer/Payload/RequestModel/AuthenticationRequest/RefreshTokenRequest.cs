using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.AuthenticationRequest
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
