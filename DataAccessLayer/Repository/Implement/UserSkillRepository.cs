using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
    {
        public UserSkillRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task UpdateUserSkillCurrentLevel(int courseId, int userId)
        {
            // Get all matching CourseSkills and UserSkills based on CourseId, SkillId, and UserId
            var matchingSkills = await _context.CourseSkills
                .Join(_context.UserSkills,
                    courseSkill => new { courseSkill.SkillId, UserId = userId },
                    userSkill => new { userSkill.SkillId, userSkill.UserId },
                    (courseSkill, userSkill) => new { CourseSkill = courseSkill, UserSkill = userSkill })
                .Where(skills => skills.CourseSkill.CourseId == courseId &&
                                 skills.CourseSkill.AfterwardLevel > skills.UserSkill.CurrentLevel)
                .ToListAsync();

            // Update the CurrentLevel in UserSkill
            foreach (var skill in matchingSkills)
            {
                skill.UserSkill.CurrentLevel = skill.CourseSkill.AfterwardLevel;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

    }
}
