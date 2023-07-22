using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Office2016.Excel;
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
                var newcourse = new Course()
                {
                    Name = request.Name,
                    PlatformName = request.PlatformName,
                    Description = request.Description,                  
                    Link = request.Link,
                    ImageURL= request.ImageURL,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Status = CommonEnums.COURSE_STATUS.ACTIVE
                };
                await _unitOfWork.CourseRepository.Add(newcourse);
                foreach(var i in request.CoursePosition)
                {
                    var newcp = new CoursePosition()
                    {
                       Position=i.Position,
                       IsCompulsory=i.IsCompulsory,
                       IsDeleted=false,
                       CreatedAt=DateTime.Now,
                       UpdatedAt=DateTime.Now,
                       CourseId=newcourse.Id
                    };
                    await _unitOfWork.CoursePositionRepository.Add(newcp);
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteCourse(int courseId)
        {

            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }
                cour.Status = CommonEnums.COURSE_STATUS.DELETED;
                await _unitOfWork.CourseRepository.Update(cour);

            } catch(Exception e)
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
                    throw new Exception("User can not enroll this course");
                }
                var newcer = new Certificate()
                {
                    Status = CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT,
                    EnrollDate = DateTime.Now,
                    CourseId = courseId,
                    UserId = userid,
                };
                await _unitOfWork.CertificateRepository.Add(newcer);             
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
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE && c.CoursePositions.Any(c=>c.IsCompulsory==true && c.Position.Equals(user.Position)),"CoursePositions");
                if (listcour == null)
                {
                    throw new Exception("No courses found");
                }
                var listresponse = listcour.Select(c =>
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
                            Position = cp.Position,
                            IsCompulsory = cp.IsCompulsory
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetCourseList(PagingRequestModel paging)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE,"CoursePositions");
                if (listcour == null)
                {
                    throw new Exception("No courses found");
                }
                var listresponse = listcour.Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Link = c.Link,
                        Name = c.Name,
                        PlatformName = c.PlatformName,
                        ImageURL= c.ImageURL,
                        coursePositions = c.CoursePositions.Select(cp =>
                        new CoursePositionResponse()
                        {
                            Id = cp.Id,
                            Position = cp.Position,
                            IsCompulsory = cp.IsCompulsory
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
                    throw new Exception("User not found");
                }
                if (user.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new Exception("User is not a trainee");
                }
                var listcour = await _unitOfWork.CourseRepository.GetrecommendCoursesForUser(user);
                if (listcour == null)
                {
                    throw new Exception("No courses found");
                }
                var listresponse = listcour.Select(c =>
                {
                    return new CourseResponse()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Link = c.Link,
                        Name = c.Name,
                        ImageURL= c.ImageURL,
                        PlatformName = c.PlatformName
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<CourseResponse>> GetEnrollCourse(int userid, PagingRequestModel paging)
        {
            try
            {
                var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE && c.Certificates.Any(c=>c.UserId==userid) , "CoursePositions","Certificates");
                if (listcour == null)
                {
                    throw new Exception("No courses found");
                }
                var listresponse = listcour.Select(c =>
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }             

        public async Task UpadateCourse(int courseId, UpdateCourseRequest request)
        {
            try
            {
                var cour = await _unitOfWork.CourseRepository.GetFirst(c => c.Id == courseId && c.Status == CommonEnums.COURSE_STATUS.ACTIVE, "CoursePositions");
                if (cour == null)
                {
                    throw new Exception("Course not found");
                }
                cour.Name = request.Name ?? cour.Name;
                cour.PlatformName = request.PlatformName ?? cour.PlatformName;
                cour.Description = request.Description ?? cour.Description;
                cour.ImageURL = request.ImageURL ?? cour.ImageURL;
                cour.Link = request.Link ?? cour.Link;
                cour.UpdatedAt = DateTime.Now;
                cour.Status = request.Status ?? cour.Status;               
                await _unitOfWork.CourseRepository.Update(cour);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
