using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.UserRequest
{
    public class UpdateUserInformationRequest
    {
        public string FullName { get; set; }
        public string Birthday { get; set; }
        public string PhoneNumber { get; set; }
    }
}
