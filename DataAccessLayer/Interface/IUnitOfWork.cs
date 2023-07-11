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
        IUserRepository UserRepository { get; }
        ICriteriaRepository CriteriaRepository { get; }
        ITrainingPlanRepository TrainingPlanRepository { get; } 
        ITrainingPlanDetailRepository TrainingPlanDetailRepository { get; } 
        IUserTrainingPlanRepository UserTrainingPlanRepository { get; }
        IOJTBatchRepository OJTBatchRepository { get; }
        ITaskRepository TaskRepository { get; }
    }
}
