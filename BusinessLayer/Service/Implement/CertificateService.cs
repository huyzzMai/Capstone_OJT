using BusinessLayer.Models.RequestModel.CertificateRequest;
using BusinessLayer.Service.Interface;
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

        public CertificateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public async Task EvaluateCertificate(EvaluateCertificateRequest request)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == request.UserId && c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role == CommonEnums.ROLE.TRAINEE, "UserSkills");
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new Exception("User is not a trainee");
                }
                var cer = await _unitOfWork.CertificateRepository.GetFirst(c => c.CourseId == request.CourseId && c.UserId == request.UserId && c.Status == CommonEnums.CERTIFICATE_STATUS.PENDING);
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == request.CourseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE, "CoursePositions", "CourseSkills");
                if (cer == null)
                {
                    throw new Exception("User do not submit or User did not enroll course");
                }
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }
                cer.Status = request.Status;
                await _unitOfWork.CertificateRepository.Update(cer);
                if (request.Status ==CommonEnums.CERTIFICATE_STATUS.APPROVED)
                {
                   await _unitOfWork.UserSkillRepository.UpdateUserSkillCurrentLevel(request.UserId, request.CourseId);
                }
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
                cer.SubmitDate = DateTime.Now;
                await _unitOfWork.CertificateRepository.Update(cer);
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
                cer.SubmitDate = DateTime.Now;
                await _unitOfWork.CertificateRepository.Update(cer);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
