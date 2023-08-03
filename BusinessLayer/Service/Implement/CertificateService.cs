using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.CertificateRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CertificateResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public CertificateService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<CertificateResponse> GetCertificateOfTrainee(int traineeId, int courseId)
        {
            try
            {
                var certificate = await _unitOfWork.CertificateRepository.GetCertificateWithUserAndCourse(traineeId, courseId);
                if (certificate == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Certificate not found!");
                }

                CertificateResponse res = new()
                {
                    CourseName = certificate.Course.Name,
                    UserName = certificate.User.Name,
                    EnrollDate = certificate.EnrollDate ?? default,
                    SubmitDate = certificate.SubmitDate ?? default,    
                    LinkCertificate = certificate.Link,  
                    Status = certificate.Status ?? default
                };
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CertificateResponse> GetCertificateOfTraineeForTrainer(int trainerId, int traineeId, int courseId)
        {
            try
            {
                var certificate = await _unitOfWork.CertificateRepository.GetCertificateWithUserAndCourse(traineeId, courseId);
                if (certificate == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Certificate not found!");
                }
                if (trainerId != certificate.User.UserReferenceId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Not your trainee's certificate!");
                }

                CertificateResponse res = new()
                {
                    CourseName = certificate.Course.Name,
                    UserName = certificate.User.Name,
                    EnrollDate = certificate.EnrollDate ?? default,
                    SubmitDate = certificate.SubmitDate ?? default,
                    LinkCertificate = certificate.Link,
                    Status = certificate.Status ?? default
                };
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CertificateResponse>> GetListCertificateOfTrainee(int traineeId, PagingRequestModel paging)
        {
            try
            {
                var items = await _unitOfWork.CertificateRepository.GetListCertificateOfTraineeWithUserAndCourse(traineeId);
                List<CertificateResponse> res = items.Select(
                    item =>
                    {
                        return new CertificateResponse()
                        {
                            CourseName = item.Course.Name,
                            UserName = item.User.Name,
                            EnrollDate = item.EnrollDate ?? default,
                            SubmitDate = item.SubmitDate ?? default,
                            LinkCertificate = item.Link,
                            Status = item.Status ?? default
                        };
                    }
                ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<CertificateResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AcceptCertificate(EvaluateCertificateRequest request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(request.UserId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found");
                }
                if (user.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "User is not a trainee");
                }

                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == request.CourseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }

                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == request.UserId && c.Status == CommonEnums.CERTIFICATE_STATUS.PENDING);
                if (cer == null)
                {
                    throw new Exception("User do not submit or User did not enroll course");
                }
                cer.Status = CommonEnums.CERTIFICATE_STATUS.APPROVED;
                await _unitOfWork.CertificateRepository.Update(cer);

                await _unitOfWork.UserSkillRepository.UpdateUserSkillCurrentLevel(request.UserId, request.CourseId);

                await _notificationService.CreateNotificaion(request.UserId, "Certificate Verified",
                      "Your certificate has been approved by the Trainer.", CommonEnums.NOTIFICATION_TYPE.UPDATE);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DenyCertificate(EvaluateCertificateRequest request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(request.UserId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found");
                }
                if (user.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "User is not a trainee");
                }

                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == request.CourseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }

                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == request.UserId && c.Status == CommonEnums.CERTIFICATE_STATUS.PENDING);
                if (cer == null)
                {
                    throw new Exception("User do not submit or User did not enroll course");
                }
                cer.Status = CommonEnums.CERTIFICATE_STATUS.DENY;
                await _unitOfWork.CertificateRepository.Update(cer);

                await _notificationService.CreateNotificaion(request.UserId, "Certificate Denied",
                      "Your certificate has been denied by the Trainer.", CommonEnums.NOTIFICATION_TYPE.UPDATE);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SubmitCertificate(int userid, SubmitCertificateRequest request)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == request.CourseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE, "CoursePositions");
                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == userid && c.Status == CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT);
                var user= await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userid);
                if (cer == null)
                {
                    throw new Exception("Certificate is already submited or User did not enroll course");
                }
                if (string.IsNullOrEmpty(request.link))
                {
                    throw new Exception("Link can not be empty");
                }
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }
                cer.Link = request.link;
                cer.Status = CommonEnums.CERTIFICATE_STATUS.PENDING;
                cer.SubmitDate = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.CertificateRepository.Update(cer);
                await _notificationService.CreateNotificaion(user.UserReferenceId ??default, "Certificate submit",
                    $"Trainne '{user.Name}' has submit certificate. Please evaluate", CommonEnums.NOTIFICATION_TYPE.CREATE);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task ReSubmitCertificate(int userid, SubmitCertificateRequest request)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == request.CourseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE, "CoursePositions");
                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == userid);
                if (cer != null && cer.Status != CommonEnums.CERTIFICATE_STATUS.DENY)
                {
                    throw new Exception("This is not a deny certificate");
                }
                if (string.IsNullOrEmpty(request.link))
                {
                    throw new Exception("Link can not be empty");
                }
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }
                cer.Link = request.link;
                cer.Status = CommonEnums.CERTIFICATE_STATUS.PENDING;
                cer.SubmitDate = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.CertificateRepository.Update(cer);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
