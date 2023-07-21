using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.RequestModel.CertificateRequest
{
    public class SubmitCertificateRequest
    {
        public string link { get; set; }
        public int CourseId { get; set; }       
    }
}
