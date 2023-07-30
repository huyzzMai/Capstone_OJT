using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface IUserSkillRepository : IGenericRepository<UserSkill>
    {
        Task UpdateUserSkillCurrentLevel(int courseId, int userId);
    }
}
