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

        public async Task<List<TaskAccomplished>> GetTaskAccomplishedOfTrainee(int userId)
        {
            var list = await _context.TaskAccomplisheds.Where(u => u.UserId == userId)
                       .ToListAsync();
            return list;
        }
    }
}
