using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ITaskRepository : IGenericRepository<TaskAccomplished>
    {
        Task<List<TaskAccomplished>> GetTaskAccomplishedOfTrainee(int userId);
    }
}
