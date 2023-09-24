using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICourseService
    {
        Task CreateCourse(CreateCourseRequest request);

        Task UpdateCourse(int courseId, UpdateCourseRequest request);

        Task CreateCourseSkill(int courseId, CourseSkillRequest request);

        Task UpdateCourseSkill(int courseId, CourseSkillRequest request);

        Task CreateCoursePosition(int courseId, CoursePositionRequest request);

        Task UpdateCoursePosition(int courseId, CoursePositionRequest request);

        Task DisableCourse(int courseId);

        Task ActiveCourse(int courseId);

        Task<BasePagingViewModel<CourseResponse>> GetCourseList(PagingRequestModel paging, string sortField, string sortOrder,string searchTerm,int? filterskill, int? filterposition,int? filterstatus);

        Task<BasePagingViewModel<CourseResponse>> GetCourseListForTrainee(PagingRequestModel paging, int userid, string sortField, string sortOrder, string searchTerm, int? filterskill, int? filterposition);

        Task<BasePagingViewModel<CourseResponse>> GetCourseListForTrainer(PagingRequestModel paging,string sortField, string sortOrder, string searchTerm, int? filterskill);

        Task<BasePagingViewModel<CourseResponse>> GetCourserecommendListForUser(int userid,PagingRequestModel paging, string searchTerm, int? filterskill);

        Task<BasePagingViewModel<CourseResponse>> GetCourseCompulsoryForUser(int userid, PagingRequestModel paging);

        Task EnrollCourse(int userid, int courseId);       
        Task<BasePagingViewModel<CourseResponse>> GetEnrollCourse(int userid, PagingRequestModel paging);

        Task<CourseDetailResponse> GetDetailCoursebyId(int courseId);

        Task AssginCourseToTrainee(int trainerId, int traineeId, int courseId);
    }
}
