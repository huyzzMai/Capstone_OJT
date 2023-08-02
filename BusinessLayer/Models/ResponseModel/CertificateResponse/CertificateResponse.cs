using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CertificateResponse
{
    public class CertificateResponse
    {
        public string CourseName { get; set; }
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "EnrollDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EnrollDate { get; set; }
        [JsonProperty(PropertyName = "SubmitDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SubmitDate { get; set; }    
        public string LinkCertificate { get; set; }
        public int Status { get; set; }     
    }
}
