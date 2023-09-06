using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.CertificateResponse
{
    public class CertificateResponse
    {
        public string CourseName { get; set; }
        public string UserName { get; set; }
        public DateTime EnrollDate { get; set; }
        public DateTime SubmitDate { get; set; }    
        public string LinkCertificate { get; set; }
        public int Status { get; set; }     
    }
}
