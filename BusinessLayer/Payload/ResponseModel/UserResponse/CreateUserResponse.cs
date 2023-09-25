using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UserResponse
{
    public class CreateUserResponse
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
