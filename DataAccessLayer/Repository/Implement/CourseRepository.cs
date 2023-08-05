using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Course>> Get(Expression<Func<Course, bool>> expression = null, params string[] includeProperties)
        {
            var result= await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.CourseSkills = _unitOfWork.CourseSkillRepository.Get(c => c.CourseId == item.Id, includeProperties: "Skill").Result.ToList();
                item.CoursePositions = _unitOfWork.CoursePositionRepository.Get(c => c.CourseId == item.Id, includeProperties: "Position").Result.ToList();
            }
            return result;
        }              
        public async Task<IEnumerable<Course>> GetrecommendCoursesForUser(User user)
        {
            var userSkills = user.UserSkills.ToList();
            var matchingCourses = await _context.Courses
                .Where(course => course.Status == CommonEnums.COURSE_STATUS.ACTIVE)
                .Include(course => course.CourseSkills)              
                .ThenInclude(cs => cs.Skill)
                .Include(course => course.CoursePositions)
                .ToListAsync();

            var filteredCourses = matchingCourses
                .Where(course => course.CourseSkills
                    .Any(cs => userSkills.Any(userSkill =>
                        cs.SkillId == userSkill.SkillId &&
                        cs.RecommendedLevel == userSkill.CurrentLevel))
                    && course.CoursePositions.Any(cp =>cp.PositionId==user.PositionId))
                .ToList();
            return filteredCourses;
        }
    }
}
