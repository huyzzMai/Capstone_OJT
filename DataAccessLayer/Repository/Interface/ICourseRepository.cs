using DataAccessLayer.Commons.CommonModels;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
      Task<IEnumerable<Course>> GetCoursesForUserSkills(List<UserSkill> userSkills);   
    }
}
