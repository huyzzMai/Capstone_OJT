using DataAccessLayer.Base;
using DataAccessLayer.Commons;
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
    public class TaskRepository : GenericRepository<TaskAccomplished>, ITaskRepository
    {
        public TaskRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<List<TaskAccomplished>> GetListTaskAccomplishedOfTrainee(int userId)
        {
            var list = await _context.TaskAccomplisheds.Where(u => u.UserId == userId)
                       .ToListAsync();
            return list;
        }

        public async Task<TaskAccomplished> GastTaskByIdAndStatusPending(string taskId)
        {
            var task = await _context.TaskAccomplisheds
                                     .Where(u => u.Id == taskId && u.Status == CommonEnums.ACCOMPLISHED_TASK_STATUS.PENDING)
                                     .Include(u => u.User)
                                     .FirstOrDefaultAsync();
            return task;
        }

    }
}
