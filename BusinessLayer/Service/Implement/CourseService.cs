using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public CourseService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task CreateCourse(CreateCourseRequest request)
        {
            try
            {
                foreach (var courseSkillRequest in request.CourseSkills)
                {
                    var skillId = courseSkillRequest.SkillId;
                    var skill = await _unitOfWork.SkillRepository.GetFirst(s => s.Id == skillId);                      
                    if (courseSkillRequest.AfterwardLevel < courseSkillRequest.RecommendedLevel)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Afterward Level can not smaller than Recommended Level");
                    }
                    if (skill == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, $"Skill with SkillId '{skillId}' not found.");
                    }
                }
                foreach (var coursePositionRequest in request.CoursePosition)
                {
                    var positionId = coursePositionRequest.PositionId;
                    var position = await _unitOfWork.SkillRepository.GetFirst(s => s.Id == positionId);                    
                    if (position == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, $"Position with positionId '{positionId}' not found.");
                    }
                }
                var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Name == request.Name);
                if (dupName != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Name already in use");
                }
                var dupLink = await _unitOfWork.CourseRepository.GetFirst(c => c.Link == request.Link);
                if (dupLink != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Link already in use");
                }
                var newcourse = new Course()
                {
                    Name = request.Name,
                    PlatformName = request.PlatformName,
                    Description = request.Description,
                    Link = request.Link,
                    ImageURL = request.ImageURL,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.COURSE_STATUS.ACTIVE
                };
                await _unitOfWork.CourseRepository.Add(newcourse);
                foreach (var i in request.CourseSkills)
                {
                    var newskill = new CourseSkill()
                    {
                        CourseId = newcourse.Id,
                        SkillId = i.SkillId,
                        AfterwardLevel = i.AfterwardLevel,
                        RecommendedLevel = i.RecommendedLevel
                    };
                    await _unitOfWork.CourseSkillRepository.Add(newskill);
                }
                foreach (var i in request.CoursePosition)
                {
                    var newcp = new CoursePosition()
                    {
                        PositionId = i.PositionId,
                        IsCompulsory = i.IsCompulsory,                                             
                        CourseId = newcourse.Id
                    };
                    await _unitOfWork.CoursePositionRepository.Add(newcp);
                }

            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DisableCourse(int courseId)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId, "Certificates");
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Course not found");
                }
                cour.Status = CommonEnums.COURSE_STATUS.INACTIVE;
                await _unitOfWork.CourseRepository.Update(cour);
                var cercour = cour.Certificates.Where(c => c.Status != CommonEnums.CERTIFICATE_STATUS.DELETED);
                foreach (var cert in cercour)
                {
                    cert.Status = CommonEnums.CERTIFICATE_STATUS.DELETED;
                    await _unitOfWork.CertificateRepository.Update(cert);
                }
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task ActiveCourse(int courseId)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId, "Certificates");
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Course not found");
                }
                cour.Status = CommonEnums.COURSE_STATUS.ACTIVE;
                await _unitOfWork.CourseRepository.Update(cour);
                var cercour = cour.Certificates.Where(c => c.Status != CommonEnums.CERTIFICATE_STATUS.DELETED);
                foreach (var cert in cercour)
                {
                    cert.Status = CommonEnums.CERTIFICATE_STATUS.DELETED;
                    await _unitOfWork.CertificateRepository.Update(cert);
                }
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task EnrollCourse(int userid, int courseId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE
                && c.Role == CommonEnums.ROLE.TRAINEE, "UserSkills");
                var course = await _unitOfWork.CourseRepository.GetFirst(c=>c.Id==courseId && c.Status==CommonEnums.COURSE_STATUS.ACTIVE);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Invalid user");
                }
                if (course == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }
                var checkmatch = course.CourseSkills.Any(c => c.RecommendedLevel > 0 
                && user.UserSkills.Any(uk => uk.SkillId == c.SkillId && uk.CurrentLevel == c.RecommendedLevel));
                if (!checkmatch)
                {
                    var checklevelzero = course.CourseSkills.Any(c => c.RecommendedLevel > 0);
                    if (checklevelzero)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "The trainee is not eligible to take this course");
                    }
                }
               
                var newcer = new Registration()
                {
                    Status = CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT,
                    EnrollDate = DateTimeService.GetCurrentDateTime(),
                    CourseId = courseId,
                    UserId = userid,
                };
                await _unitOfWork.CertificateRepository.Add(newcer);
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AssginCourseToTrainee(int trainerId, int traineeId, int courseId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == traineeId && c.Status == CommonEnums.USER_STATUS.ACTIVE
                && c.Role == CommonEnums.ROLE.TRAINEE, "UserSkills");
                var course = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Invalid user");
                }
                if (course == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }
                if (trainerId != user.UserReferenceId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "This is not your assigned Trainee!");
                }
                // Add student to code here
                ;

                // Create notification for assigned trainee
                await _notificationService.CreateNotificaion(traineeId, "New Course Assigned For You",
                      "Your have been assigned a new course by your trainer.", CommonEnums.NOTIFICATION_TYPE.COURSE_TYPE);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetCourseCompulsoryForUser(int userid, PagingRequestModel paging)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role == CommonEnums.ROLE.TRAINEE);
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE && c.CoursePositions.Any(c => c.IsCompulsory == true && c.Position.Equals(user.Position)), "CoursePositions", "CourseSkills");
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                var listresponse = listcour.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        ImageURL = c.ImageURL,
                        Link = c.Link,
                        Name = c.Name,
                        PlatformName = c.PlatformName,
                        coursePositions = c.CoursePositions.Select(cp =>
                        new CoursePositionResponse()
                        {
                            PositionId = cp.PositionId,
                            PositionName = cp.Position.Name,
                            IsCompulsory = cp.IsCompulsory

                        }).ToList(),
                        courseSkills = c.CourseSkills.Select(cp =>
                        new CourseSkillResponse()
                        {
                            SkillId = cp.SkillId,
                            SkillName=cp.Skill.Name,
                            AfterwardLevel = cp.AfterwardLevel,
                            RecommendedLevel = cp.RecommendedLevel
                        }).ToList()
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                  .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<CourseResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Course> SearchCourses(string searchTerm, int? filterskill, int? filterposition, int? filterstatus, List<Course> courselist)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
            }

            var query = courselist.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.PlatformName.ToLower().Contains(searchTerm) ||
                    c.CourseSkills.Any(cs => cs.Skill.Name.ToLower().Contains(searchTerm))
                );
            }
            if (filterstatus != null)
            {
                query = query.Where(c => c.Status == filterstatus);
            }
            if (filterposition != null)
            {
                query = query.Where(c => c.CoursePositions.Any(c => c.PositionId == filterposition));
            }
            if (filterskill != null)
            {
                query = query.Where(c => c.CourseSkills.Any(c => c.SkillId == filterskill));
            }
            return query.ToList();
        }
        public async Task<BasePagingViewModel<CourseResponse>> GetCourseList(PagingRequestModel paging, string sortField, string sortOrder, string searchTerm, int? filterskill, int? filterposition, int? filterstatus)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(expression: null, "CoursePositions", "CourseSkills", "Certificates");
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                if (!string.IsNullOrEmpty(searchTerm) || filterskill != null || filterposition != null || filterstatus != null)
                {
                    listcour = SearchCourses(searchTerm, filterskill, filterposition, filterstatus, listcour.ToList());
                }
                var listresponse = listcour.OrderByDescending(c => c.CreatedAt).Select(c =>
            {
                return new CourseResponse()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Link = c.Link,
                    Name = c.Name,
                    PlatformName = c.PlatformName,
                    ImageURL = c.ImageURL,
                    TotalEnrollment = c.Certificates.Count,
                    TotalActiveEnrollment = c.Certificates.Where(c => c.Status != CommonEnums.CERTIFICATE_STATUS.DELETED).Count(),
                    coursePositions = c.CoursePositions.Select(cp =>
                    new CoursePositionResponse()
                    {
                        PositionId = cp.Id,
                        PositionName=cp.Position.Name,
                        IsCompulsory = cp.IsCompulsory

                    }).ToList(),
                    courseSkills = c.CourseSkills.Select(cp =>
                    new CourseSkillResponse()
                    {
                        SkillId = cp.SkillId,
                        SkillName = cp.Skill.Name,
                        AfterwardLevel = cp.AfterwardLevel,
                        RecommendedLevel = cp.RecommendedLevel
                    }).ToList()
                };
            }
            ).ToList();
                listresponse = SortingHelper.ApplySorting(listresponse.AsQueryable(), sortField, sortOrder).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<CourseResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetCourserecommendListForUser(int userid, PagingRequestModel paging, string searchTerm, int? filterskill)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role == CommonEnums.ROLE.TRAINEE, "UserSkills");
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found");
                }
                var listcour = await _unitOfWork.CourseRepository.GetrecommendCoursesForUser(user);
                if (!string.IsNullOrEmpty(searchTerm) || filterskill != null )
                {
                    listcour = SearchCourses(searchTerm, filterskill, null, null, listcour.ToList());
                }
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                var listresponse = listcour.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Link = c.Link,
                        Name = c.Name,
                        ImageURL = c.ImageURL,
                        PlatformName = c.PlatformName,
                        coursePositions = c.CoursePositions.Select(cp =>
                        new CoursePositionResponse()
                        {
                            PositionId = cp.Id,
                            PositionName = cp.Position.Name,
                            IsCompulsory = cp.IsCompulsory

                        }).ToList(),
                        courseSkills = c.CourseSkills.Select(cp =>
                        new CourseSkillResponse()
                        {
                            SkillId = cp.SkillId,
                            SkillName = cp.Skill.Name,
                            AfterwardLevel = cp.AfterwardLevel,
                            RecommendedLevel = cp.RecommendedLevel

                        }).ToList()
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                  .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<CourseResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CourseDetailResponse> GetDetailCoursebyId(int courseId)
        {
            try
            {
                var c = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId, "CoursePositions", "CourseSkills","Certificates");
                if (c == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }
                var courdetail = new CourseDetailResponse()
                {
                    Id = c.Id,
                    Description = c.Description,
                    ImageURL = c.ImageURL,
                    Link = c.Link,
                    Name = c.Name,
                    PlatformName = c.PlatformName,
                    Status = c.Status,
                    TotalEnrollment = c.Certificates.Count,
                    TotalActiveEnrollment = c.Certificates.Where(c => c.Status != CommonEnums.CERTIFICATE_STATUS.DELETED).Count(),
                    CreatedAt = DateTimeService.ConvertToDateString(c.CreatedAt),
                    UpdatedAt = DateTimeService.ConvertToDateString(c.UpdatedAt),
                    CoursePositions = c.CoursePositions.Select(cp =>
                    new CoursePositionResponse()
                    {
                        PositionId = cp.Id,
                        PositionName = cp.Position.Name,
                        IsCompulsory = cp.IsCompulsory

                    }).ToList(),
                    CourseSkills = c.CourseSkills.Select(cp =>
                    new CourseSkillResponse()
                    {
                        SkillId = cp.SkillId,
                        SkillName = cp.Skill.Name,
                        AfterwardLevel = cp.AfterwardLevel,
                        RecommendedLevel = cp.RecommendedLevel
                    }).ToList()
                };
                return courdetail;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetEnrollCourse(int userid, PagingRequestModel paging)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE && c.Certificates.Any(c => c.UserId == userid), "CoursePositions", "Certificates");
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                var listresponse = listcour.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Link = c.Link,
                        Name = c.Name,
                        PlatformName = c.PlatformName,
                        ImageURL = c.ImageURL
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                  .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<CourseResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateCourse(int courseId, UpdateCourseRequest request)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId);
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                if (request.Name.ToLower() != cour.Name.ToLower())
                {
                    var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Name == request.Name);
                    if (dupName != null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Name already in use");
                    }
                }
                if (request.Link != cour.Link)
                {
                    var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Link == request.Link);
                    if (dupName != null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Link already in use");
                    }
                }
                cour.Name = request.Name;
                cour.PlatformName = request.PlatformName;
                cour.Description = request.Description;
                cour.ImageURL = request.ImageURL;
                cour.Link = request.Link;
                cour.UpdatedAt = DateTime.UtcNow.AddHours(7);
                cour.Status = request.Status;
                await _unitOfWork.CourseRepository.Update(cour);
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteCourseSkill(int courseId, int skillid)
        {
            try
            {
                var cour = await _unitOfWork.CourseSkillRepository.GetFirst(c => c.CourseId == courseId && c.SkillId == skillid);
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "No courses skill found");
                }
                await _unitOfWork.CourseSkillRepository.Delete(cour);
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetCourseListForTrainee(PagingRequestModel paging,int userid ,string sortField, string sortOrder, string searchTerm, int? filterskill, int? filterposition, int? filterstatus)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(c=>c.Status==CommonEnums.COURSE_STATUS.ACTIVE
                && (!c.Certificates.Any()|| c.Certificates.All(c=>c.UserId!=userid))
                , "CoursePositions", "CourseSkills", "Certificates");
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                if (!string.IsNullOrEmpty(searchTerm) || filterskill != null || filterposition != null || filterstatus != null)
                {
                    listcour = SearchCourses(searchTerm, filterskill, filterposition, filterstatus, listcour.ToList());
                }
                var listresponse = listcour.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Link = c.Link,
                        Name = c.Name,
                        PlatformName = c.PlatformName,
                        ImageURL = c.ImageURL,
                        TotalEnrollment = c.Certificates.Count,
                        TotalActiveEnrollment = c.Certificates.Where(c => c.Status != CommonEnums.CERTIFICATE_STATUS.DELETED).Count(),
                        coursePositions = c.CoursePositions.Select(cp =>
                        new CoursePositionResponse()
                        {
                            PositionId = cp.Id,
                            PositionName = cp.Position.Name,
                            IsCompulsory = cp.IsCompulsory

                        }).ToList(),
                        courseSkills = c.CourseSkills.Select(cp =>
                        new CourseSkillResponse()
                        {
                            SkillId = cp.SkillId,
                            SkillName = cp.Skill.Name,
                            AfterwardLevel = cp.AfterwardLevel,
                            RecommendedLevel = cp.RecommendedLevel
                        }).ToList()
                    };
                }
            ).ToList();
                listresponse = SortingHelper.ApplySorting(listresponse.AsQueryable(), sortField, sortOrder).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<CourseResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
