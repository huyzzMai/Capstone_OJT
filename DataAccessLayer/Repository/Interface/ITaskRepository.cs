using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ITaskRepository : IGenericRepository<TaskAccomplished>
    {
        Task<List<TaskAccomplished>> GetListTaskAccomplishedOfTrainee(int userId);

        Task<TaskAccomplished> GetTaskAccomplishedById(int id); 

        Task<TaskAccomplished> GastTaskByIdAndStatusPending(int taskId);

        Task<TaskAccomplished> GetMatchingTask(string trelloTaskId, int userId);  

        Task<List<TaskAccomplished>> GetListTaskPendingOfTrainee(int userId);

        Task<List<TaskAccomplished>> GetListTaskAccomplishedDoneOfTrainee(int userId);

        Task<List<TaskAccomplished>> GetListTaskAccomplishedFailedOfTrainee(int userId);
    }
}
