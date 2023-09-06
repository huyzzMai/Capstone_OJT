using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.ConfigRequest
{
    public class UpdateConfigRequest
    {
        [Required]
        public int value { get; set; }
    }
}
