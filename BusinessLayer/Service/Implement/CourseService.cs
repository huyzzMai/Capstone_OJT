using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
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
        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Name == request.Name && c.Status != CommonEnums.COURSE_STATUS.DELETED);
                if (dupName != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Name already in use");
                }
                var dupLink = await _unitOfWork.CourseRepository.GetFirst(c => c.Link == request.Link && c.Status != CommonEnums.COURSE_STATUS.DELETED);
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
                        Position = i.Position,
                        IsCompulsory = i.IsCompulsory,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),
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

        public async Task DeleteCourse(int courseId)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId && c.Status != CommonEnums.COURSE_STATUS.DELETED, "Certificates");
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Course not found");
                }
                cour.Status = CommonEnums.COURSE_STATUS.DELETED;
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
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role == CommonEnums.ROLE.TRAINEE);
                var ismatch = await _unitOfWork.CourseRepository.GetFirst(c => c.CoursePositions.Any(c => c.Position.Equals(user.Position)));
                if (ismatch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "User can not enroll this course");
                }
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Invalid user");
                }
                if (await _unitOfWork.CourseRepository.Get(c => c.Id == courseId) == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Course not found");
                }
                var newcer = new Certificate()
                {
                    Status = CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT,
                    EnrollDate = DateTime.UtcNow.AddHours(7),
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
                            Id = cp.Id,
                            Position = cp.Position ?? default(int),
                            IsCompulsory = cp.IsCompulsory
                        }).ToList(),
                        courseSkills = c.CourseSkills.Select(cp =>
                        new CourseSkillResponse()
                        {
                            SkillId = cp.SkillId,
                            AfterwardLevel = cp.AfterwardLevel,
                            RecommendedLevel = cp.RecommendedLevel
                        }).ToList()
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;

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
        public List<Course> SearchCourses(string searchTerm, string filterskill, int? filterposition, int? filterstatus, List<Course> courselist)
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
                query = query.Where(c => c.CoursePositions.Any(c => c.Position == filterposition));
            }
            if (!string.IsNullOrEmpty(filterskill))
            {
                filterskill = filterskill.ToLower();
                query = query.Where(c =>
                    c.CourseSkills.Any(cs => cs.Skill.Name.ToLower().Contains(filterskill))
                );
            }
            return query.ToList();
        }
        public async Task<BasePagingViewModel<CourseResponse>> GetCourseList(PagingRequestModel paging, string sortField, string sortOrder, string searchTerm, string filterskill, int? filterposition, int? filterstatus)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status != CommonEnums.COURSE_STATUS.DELETED, "CoursePositions", "CourseSkills", "Certificates");
                if (listcour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(filterskill) || filterposition != null || filterstatus != null)
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
                        Id = cp.Id,
                        Position = cp.Position ?? default(int),
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

        public async Task<BasePagingViewModel<CourseResponse>> GetCourserecommendListForUser(int userid, PagingRequestModel paging)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role == CommonEnums.ROLE.TRAINEE, "UserSkills");
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found");
                }
                if (user.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "User is not a trainee");
                }
                var listcour = await _unitOfWork.CourseRepository.GetrecommendCoursesForUser(user);
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
                            Id = cp.Id,
                            Position = cp.Position ?? default(int),
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
                var c = await _unitOfWork.CourseRepository.GetFirst(c => c.Status != CommonEnums.COURSE_STATUS.DELETED && c.Id == courseId, "CoursePositions", "CourseSkills");
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
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    CoursePositions = c.CoursePositions.Select(cp =>
                    new CoursePositionResponse()
                    {
                        Id = cp.Id,
                        Position = cp.Position ?? default(int),
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
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId && c.Status != CommonEnums.COURSE_STATUS.DELETED);
                if (cour == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "No courses found");
                }
                if (request.Name.ToLower() != cour.Name.ToLower())
                {
                    var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Name == request.Name && c.Status != CommonEnums.COURSE_STATUS.DELETED);
                    if (dupName != null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Name already in use");
                    }
                }
                if (request.Link != cour.Link)
                {
                    var dupName = await _unitOfWork.CourseRepository.GetFirst(c => c.Link == request.Link && c.Status != CommonEnums.COURSE_STATUS.DELETED);
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
    }
}
