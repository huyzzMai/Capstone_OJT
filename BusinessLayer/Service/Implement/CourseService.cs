using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.ResponseModel.CourseResponse;
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
                    //IsCompulsory = request.IsCompulsory,
                    Link = request.Link,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    //IsDeleted = false,
                    Status = CommonEnums.COURSE_STATUS.ACTIVE
                };
                await _unitOfWork.CourseRepository.Add(newcourse);
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

        //public async Task<IEnumerable<CourseResponse>> GetCourseCompulsoryForUser()
        //{
        //    try
        //    {
        //        var listcour = await _unitOfWork.CourseRepository.Get(c=>c.Status==CommonEnums.COURSE_STATUS.ACTIVE && c.IsCompulsory==true);
        //        if(listcour==null)
        //        {
        //            throw new Exception("No courses found");
        //        }
        //        var listresponse= listcour.Select(c=>
        //        {
        //            return new CourseResponse()
        //            {
        //                Id= c.Id,
        //                Description= c.Description,
        //                IsCompulsory= c.IsCompulsory,
        //                Link = c.Link,
        //                Name= c.Name,
        //                PlatformName = c.PlatformName
        //            };
        //        }
        //        ).ToList();
        //        return  listresponse;
        //    } catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        //public async Task<IEnumerable<CourseResponse>> GetCourseList()
        //{
        //    try
        //    {
        //        var listcour = await _unitOfWork.CourseRepository.Get(c => c.Status == CommonEnums.COURSE_STATUS.ACTIVE);
        //        if (listcour == null)
        //        {
        //            throw new Exception("No courses found");
        //        }
        //        var listresponse = listcour.Select(c =>
        //        {
        //            return new CourseResponse()
        //            {
        //                Id = c.Id,
        //                Description = c.Description,
        //                IsCompulsory = c.IsCompulsory,
        //                Link = c.Link,
        //                Name = c.Name,
        //                PlatformName = c.PlatformName
        //            };
        //        }
        //        ).ToList();
        //        return listresponse;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        //public async Task<IEnumerable<CourseResponse>> GetCourserecommendListForUser(int userid)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==userid&&c.Status==CommonEnums.USER_STATUS.ACTIVE,"UserSkills");
        //        if(user == null)
        //        {
        //            throw new Exception("User not found");
        //        }
        //        if (user.Role != CommonEnums.ROLE.TRAINEE)
        //        {
        //            throw new Exception("User is not a trainee");
        //        }
        //        var listcour = await _unitOfWork.CourseRepository.GetCoursesForUserSkills(user.UserSkills.ToList());
        //        if (listcour == null)
        //        {
        //            throw new Exception("No courses found");
        //        }
        //        var listresponse = listcour.Select(c =>
        //        {
        //            return new CourseResponse()
        //            {
        //                Id = c.Id,
        //                Description = c.Description,
        //                IsCompulsory = c.IsCompulsory,
        //                Link = c.Link,
        //                Name = c.Name,
        //                PlatformName = c.PlatformName
        //            };
        //        }
        //        ).ToList();
        //        return listresponse;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        //public async Task UpadateCourse(int courseId,UpdateCourseRequest request)
        //{
        //    try
        //    {
        //        var cour = await _unitOfWork.CourseRepository.GetFirst(c=>c.Id==courseId && c.Status==CommonEnums.COURSE_STATUS.ACTIVE);
        //        if(cour == null)
        //        {
        //            throw new Exception("Course not found");
        //        }
        //        cour.Name = request.Name??cour.Name;
        //        cour.PlatformName = request.PlatformName??cour.PlatformName;
        //        cour.Description = request.Description ?? cour.Description;
        //        cour.IsCompulsory = request.IsCompulsory ?? cour.IsCompulsory;
        //        cour.Link = request.Link ?? cour.Link;
        //        cour.UpdatedAt = DateTime.Now;
        //        cour.Status =request.Status ?? cour.Status;              
        //        await _unitOfWork.CourseRepository.Update(cour);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
    }
}
