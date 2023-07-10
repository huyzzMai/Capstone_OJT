using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.ResponseModel.CourseResponse;
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

        Task UpadateCourse(int courseId,UpdateCourseRequest request);

        Task DeleteCourse(int courseId);

        Task<IEnumerable<CourseResponse>> GetCourseList();

        Task<IEnumerable<CourseResponse>> GetCourserecommendListForUser(int userid);

        Task<IEnumerable<CourseResponse>> GetCourseCompulsoryForUser();
    }
}
