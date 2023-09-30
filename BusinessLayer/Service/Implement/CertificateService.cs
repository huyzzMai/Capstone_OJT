using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CertificateRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CertificateResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TraineeCertificateResponse> GetCertificateOfTrainee(int traineeId, int courseId)
        {
            try
            {
                var certificate = await _unitOfWork.CertificateRepository.GetCertificateWithUserAndCourse(traineeId, courseId);
                if (certificate == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Certificate not found!");
                }

                TraineeCertificateResponse res = new()
                {
                    CourseId = certificate.CourseId,
                    CourseName = certificate.Course.Name,
                    CourseImg = certificate.Course.ImageURL,
                    UserName = certificate.User.FirstName,
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

        public async Task<TrainerCertificateResponse> GetCertificateOfTraineeForTrainer(int trainerId, int traineeId, int courseId)
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

                TrainerCertificateResponse res = new()
                {
                    CourseId = certificate.CourseId,
                    CourseName = certificate.Course.Name,
                    CourseImg = certificate.Course.ImageURL,
                    EnrollDate = certificate.EnrollDate ?? default,
                    SubmitDate = certificate.SubmitDate ?? default,
                    LinkCertificate = certificate.Link,
                    Status = certificate.Status ?? default,
                    UserId = certificate.UserId,
                    FirstName = certificate.User.FirstName,
                    LastName = certificate.User.LastName,
                    AvatarURL = certificate.User.AvatarURL
                };
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<TraineeCertificateResponse>> GetListCertificateOfTrainee(int traineeId, PagingRequestModel paging, int? status)
        {
            try
            {
                var items = await _unitOfWork.CertificateRepository.GetListCertificateOfTraineeWithUserAndCourse(traineeId);
                if (status != null)
                {
                    items = items.Where(x => x.Status == status).ToList();   
                }
                List<TraineeCertificateResponse> res = items.Select(
                    certificate =>
                    {
                        return new TraineeCertificateResponse()
                        {
                            CourseId = certificate.CourseId,
                            CourseName = certificate.Course.Name,
                            CourseImg = certificate.Course.ImageURL,
                            UserName = certificate.User.FirstName,
                            EnrollDate = certificate.EnrollDate ?? default,
                            SubmitDate = certificate.SubmitDate ?? default,
                            LinkCertificate = certificate.Link,
                            Status = certificate.Status ?? default
                        };
                    }
                ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TraineeCertificateResponse>()
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

        public async Task<BasePagingViewModel<TrainerCertificateResponse>> GetListCertificateOfTraineeForTrainer(int traineeId, PagingRequestModel paging, int? status)
        {
            try
            {
                var items = await _unitOfWork.CertificateRepository.GetListCertificateOfTraineeWithUserAndCourse(traineeId);
                if (status != null)
                {
                    items = items.Where(x => x.Status == status).ToList();
                }
                List<TrainerCertificateResponse> res = items.Select(
                    certificate =>
                    {
                        return new TrainerCertificateResponse()
                        {
                            CourseId = certificate.CourseId,
                            CourseName = certificate.Course.Name,
                            CourseImg = certificate.Course.ImageURL,
                            EnrollDate = certificate.EnrollDate ?? default,
                            SubmitDate = certificate.SubmitDate ?? default,
                            LinkCertificate = certificate.Link,
                            Status = certificate.Status ?? default,
                            UserId = certificate.UserId,
                            FirstName = certificate.User.FirstName,
                            LastName = certificate.User.LastName,
                            AvatarURL = certificate.User.AvatarURL
                        };
                    }
                ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TrainerCertificateResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<BasePagingViewModel<TrainerCertificateResponse>> GetListCertificatePendingOffAllTraineesForTrainer(int trainerId, PagingRequestModel paging)
        {
            try
            {
                var trainer = await _unitOfWork.UserRepository.GetTrainerWithListTraineeByIdAndStatusActive(trainerId);
                if (trainer == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found");
                }
                if (trainer.Trainees == null || trainer.Trainees.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Trainer does not have any trainees!");
                }

                List<Registration> certis = new();
                foreach (var trainee in trainer.Trainees)
                {
                    var items = await _unitOfWork.CertificateRepository.GetlistCertificatePendingOfTraineeWithUserAndCourse(trainee.Id);
                    certis.AddRange(items);
                }
                certis = certis.OrderByDescending(x => x.SubmitDate).ToList();

                List<TrainerCertificateResponse> res = certis.Select(
                    certificate =>
                    {
                        return new TrainerCertificateResponse()
                        {
                            CourseId = certificate.CourseId,
                            CourseName = certificate.Course.Name,
                            CourseImg = certificate.Course.ImageURL,
                            EnrollDate = certificate.EnrollDate ?? default,
                            SubmitDate = certificate.SubmitDate ?? default,
                            LinkCertificate = certificate.Link,
                            Status = certificate.Status ?? default,
                            UserId = certificate.UserId,
                            FirstName = certificate.User.FirstName,
                            LastName = certificate.User.LastName,
                            AvatarURL = certificate.User.AvatarURL
                        };
                    }
                ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TrainerCertificateResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }

                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == request.UserId && c.Status == CommonEnums.CERTIFICATE_STATUS.PENDING);
                if (cer == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User do not submit or User did not enroll course");
                }
                cer.Status = CommonEnums.CERTIFICATE_STATUS.APPROVED;
                await _unitOfWork.CertificateRepository.Update(cer);

                var listSkill = await _unitOfWork.UserSkillRepository.GetListSkillOfCourse(request.CourseId); 
                var listUserSKill = await _unitOfWork.UserSkillRepository.GetListUserSkill(request.UserId); 

                foreach ( var skill in listSkill)
                {
                    var check = listUserSKill.FirstOrDefault(u => u.SkillId == skill.SkillId);
                    if(check == null)
                    {
                        UserSkill us = new()
                        {
                            SkillId = skill.SkillId,
                            UserId = request.UserId,    
                            CurrentLevel = skill.AfterwardLevel,
                            InitLevel = 0
                        };
                        await _unitOfWork.UserSkillRepository.Add(us);  
                    }
                    else
                    {
                        check.CurrentLevel = skill.AfterwardLevel;
                        await _unitOfWork.UserSkillRepository.Update(check);
                    }
                }
                //await _unitOfWork.UserSkillRepository.UpdateUserSkillCurrentLevel(request.UserId, request.CourseId);

                await _notificationService.CreateNotificaion(request.UserId, "Xử Lý Chứng Chỉ",
                      "Chứng chỉ của bạn đã được xác nhận bởi đào tạo viên.", CommonEnums.NOTIFICATION_TYPE.CERTIFICATE_TYPE);
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

                await _notificationService.CreateNotificaion(request.UserId, "Xử Lý Chứng Chỉ",
                      "Chứng chỉ của bạn đã bị từ chối bởi đào tạo viên.", CommonEnums.NOTIFICATION_TYPE.CERTIFICATE_TYPE);
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
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Certificate is already submited or User did not enroll course");
                }
                if (string.IsNullOrEmpty(request.link))
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Link can not be empty");
                }
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }
                cer.Link = request.link;
                cer.Status = CommonEnums.CERTIFICATE_STATUS.PENDING;
                cer.SubmitDate = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.CertificateRepository.Update(cer);
                await _notificationService.CreateNotificaion(user.UserReferenceId ??default, "Nộp Chứng Chỉ",
                    $"Thực tập viên '{user.FirstName}' đã nộp bằng. Hãy vào xác nhận.", CommonEnums.NOTIFICATION_TYPE.CERTIFICATE_TYPE);

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
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userid);
                if (cer == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Not found certificate");
                }
                if (cer.Status != CommonEnums.CERTIFICATE_STATUS.DENY && cer.Status != CommonEnums.CERTIFICATE_STATUS.PENDING)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "This is not a valid certificate to re-submit");
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
                await _notificationService.CreateNotificaion(user.UserReferenceId ?? default, "Nộp Lại Chứng Chỉ",
                    $"Thực tập viên '{user.FirstName}' đã nộp bằng. Hãy vào xác nhận.", CommonEnums.NOTIFICATION_TYPE.CERTIFICATE_TYPE);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
