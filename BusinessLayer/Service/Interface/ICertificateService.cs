using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CertificateRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CertificateResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICertificateService
    {
        Task<TraineeCertificateResponse> GetCertificateOfTrainee(int traineeId, int courseId);
        Task<TrainerCertificateResponse> GetCertificateOfTraineeForTrainer(int trainerId, int traineeId, int courseId);
        Task<BasePagingViewModel<TraineeCertificateResponse>> GetListCertificateOfTrainee(int traineeId, PagingRequestModel paging);
        Task<BasePagingViewModel<TrainerCertificateResponse>> GetListCertificateOfTraineeForTrainer(int traineeId, PagingRequestModel paging);
        Task AcceptCertificate(EvaluateCertificateRequest request);
        Task DenyCertificate(EvaluateCertificateRequest request);
        Task SubmitCertificate(int userid, SubmitCertificateRequest request);
        Task ReSubmitCertificate(int userid, SubmitCertificateRequest request);
    }
}
