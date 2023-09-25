using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.RequestModel.CertificateRequest
{
    public class SubmitCertificateRequest
    {
        [Required]
        public string link { get; set; }
        [Required]
        public int CourseId { get; set; }       
    }
}
