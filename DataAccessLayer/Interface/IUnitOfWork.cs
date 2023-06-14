using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IUnitOfWork
    {
        ICriteriaRepository CriteriaRepository { get; }
        //IAccountRepository AccountRepository { get; }
        //IApplicationRepository ApplicationRepository { get; }
        //ICategoryRepository CategoryRepository { get; }
        //IInterviewRepository InterviewRepository { get; }
        //ILevelRepository LevelRepository { get; }
        //IPostRepository PostRepository { get; }
        //IPostSkillRepository PostSkillRepository { get; }
        //IRoleRepository RoleRepository { get; }
        //ISkillRepository SkillRepository { get; }
        //ISlotRepository SlotRepository { get; }
        //IUserSkillRepository UserSkillRepository { get; }        
    }
}
