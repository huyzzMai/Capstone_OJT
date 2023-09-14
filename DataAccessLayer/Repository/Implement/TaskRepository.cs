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

        public async Task<TaskAccomplished> GetTaskAccomplishedById(int id)
        {
            var task = await _context.TaskAccomplisheds
                                     .Where(u => u.Id == id) 
                                     .Include(u => u.User)
                                     .FirstOrDefaultAsync();    
            return task;    
        }

        public async Task<TaskAccomplished> GastTaskByIdAndStatusPending(int taskId)
        {
            var task = await _context.TaskAccomplisheds
                                     .Where(u => u.Id == taskId && u.Status == CommonEnums.ACCOMPLISHED_TASK_STATUS.PENDING)
                                     .Include(u => u.User)
                                     .FirstOrDefaultAsync();
            return task;
        }

        public async Task<TaskAccomplished> GetTaskAccomplishedByTrelloTaskId(string trelloTaskId)
        {
            var task = await _context.TaskAccomplisheds
                                     .Where(u => u.TrelloTaskId == trelloTaskId)
                                     .Include(u => u.User)
                                     .FirstOrDefaultAsync();
            return task;
        }

        public async Task<TaskAccomplished> GetMatchingTask(string trelloTaskId, int userId)
        {
            var task = await _context.TaskAccomplisheds
                                     .Where(u => u.TrelloTaskId == trelloTaskId && u.UserId == userId)
                                     .FirstOrDefaultAsync();
            return task;
        }

        public async Task<List<TaskAccomplished>> GetListTaskPendingOfTrainee(int userId)
        {
            var list = await _context.TaskAccomplisheds
                       .Where(u => u.UserId == userId && u.Status == CommonEnums.ACCOMPLISHED_TASK_STATUS.PENDING)
                       .ToListAsync();
            return list;
        }

        public async Task<List<TaskAccomplished>> GetListTaskAccomplishedDoneOfTrainee(int userId)
        {
            var list = await _context.TaskAccomplisheds
                       .Where(u => u.UserId == userId && u.Status == CommonEnums.ACCOMPLISHED_TASK_STATUS.DONE)
                       .ToListAsync();
            return list;
        }

        public async Task<List<TaskAccomplished>> GetListTaskAccomplishedFailedOfTrainee(int userId)
        {
            var list = await _context.TaskAccomplisheds
                       .Where(u => u.UserId == userId && u.Status == CommonEnums.ACCOMPLISHED_TASK_STATUS.FAILED)
                       .ToListAsync();
            return list;
        }
    }
}
