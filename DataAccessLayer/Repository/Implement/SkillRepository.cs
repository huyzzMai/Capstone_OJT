using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Skill>> Get(Expression<Func<Skill, bool>> expression = null, params string[] includeProperties)
        {
            var result = await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.CourseSkills = _unitOfWork.CourseSkillRepository.Get(c => c.SkillId == item.Id, includeProperties: "Course").Result.ToList();
            }
            foreach (var item in result)
            {
                item.UserSkills = _unitOfWork.UserSkillRepository.Get(c => c.SkillId == item.Id, includeProperties: "User").Result.ToList();
            }
            return result;
        }
    }
}
