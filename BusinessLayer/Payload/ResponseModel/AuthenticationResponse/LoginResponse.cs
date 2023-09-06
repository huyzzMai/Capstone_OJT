using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.AuthenticationResponse
{
    public class LoginResponse
    {
        public string UserId { get; set; }    
        public string Role { get; set; }
    }
}
