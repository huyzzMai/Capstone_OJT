using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Implement;
using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OJTDbContext _context;
        public UnitOfWork(OJTDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => new UserRepository(_context, this);

        public ICriteriaRepository CriteriaRepository => new CriteriaRepository(_context, this);

        public ITrainingPlanRepository TrainingPlanRepository => new TrainingPlanRepository(_context, this);

        public ITrainingPlanDetailRepository TrainingPlanDetailRepository => new TrainingPlanDetailRepository(_context, this);

        public IUserTrainingPlanRepository UserTrainingPlanRepository => new UserTrainingPlanRepository(_context, this);

        public IOJTBatchRepository OJTBatchRepository => new OJTBatchRepository(_context, this);

        public ITaskRepository TaskRepository => new TaskRepository(_context, this);

        public ICourseRepository CourseRepository => new CourseRepository(_context, this);

        public ICoursePositionRepository CoursePositionRepository => new CoursePositionRepository(_context, this);

        public ICertificateRepository CertificateRepository => new CertificateRepository(_context, this);   

        public IUserSkillRepository UserSkillRepository=> new UserSkillRepository(_context, this);
    }
}
