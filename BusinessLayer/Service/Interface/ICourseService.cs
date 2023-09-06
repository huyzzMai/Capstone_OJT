﻿using BusinessLayer.Payload.RequestModel;
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

        Task DeleteCourseSkill(int courseId, int skillid);

        Task DeleteCourse(int courseId);

        Task<BasePagingViewModel<CourseResponse>> GetCourseList(PagingRequestModel paging, string sortField, string sortOrder,string searchTerm,int? filterskill, int? filterposition,int? filterstatus);

        Task<BasePagingViewModel<CourseResponse>> GetCourserecommendListForUser(int userid,PagingRequestModel paging);

        Task<BasePagingViewModel<CourseResponse>> GetCourseCompulsoryForUser(int userid, PagingRequestModel paging);

        Task EnrollCourse(int userid, int courseId);       
        Task<BasePagingViewModel<CourseResponse>> GetEnrollCourse(int userid, PagingRequestModel paging);

        Task<CourseDetailResponse> GetDetailCoursebyId(int courseId);
    }
}
