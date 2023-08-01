using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.CertificateRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CertificateResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICertificateService
    {
        Task<CertificateResponse> GetCertificateOfTrainee(int traineeId, int courseId);
        Task<CertificateResponse> GetCertificateOfTraineeForTrainer(int trainerId, int traineeId, int courseId);
        Task<BasePagingViewModel<CertificateResponse>> GetListCertificateOfTrainee(int traineeId, PagingRequestModel paging);
        Task AcceptCertificate(EvaluateCertificateRequest request);
        Task DenyCertificate(EvaluateCertificateRequest request);
        Task SubmitCertificate(int userid, SubmitCertificateRequest request);
        Task ReSubmitCertificate(int userid, SubmitCertificateRequest request);
    }
}
