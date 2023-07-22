using BusinessLayer.Models.RequestModel.CertificateRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICertificateService
    {
        Task EvaluateCertificate(EvaluateCertificateRequest request);
        Task SubmitCertificate(int userid, SubmitCertificateRequest request);
    }
}
