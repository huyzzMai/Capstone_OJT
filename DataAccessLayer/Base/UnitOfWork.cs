using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OJTDbContext _context;
        public UnitOfWork(OJTDbContext context)
        {
            _context = context;
        }

        //public IAccountRepository AccountRepository => new AccountRepository(_context, this);

        //public IApplicationRepository ApplicationRepository => new ApplicationRepository(_context, this);

        //public ICategoryRepository CategoryRepository => new CategoryRepository(_context, this);

        //public IInterviewRepository InterviewRepository => new InterviewRepository(_context, this);

        //public ILevelRepository LevelRepository => new LevelRepository(_context, this);

        //public IPostRepository PostRepository =>  new PostRepository(_context, this);

        //public IPostSkillRepository PostSkillRepository => new PostSkillRepository(_context, this);

        //public IRoleRepository RoleRepository => new RoleRepository(_context, this);

        //public ISkillRepository SkillRepository => new SkillRepository(_context, this);

        //public ISlotRepository SlotRepository => new SlotRepository(_context, this);

        //public IUserSkillRepository UserSkillRepository => new UserSkillRepository(_context, this);
    }
}
