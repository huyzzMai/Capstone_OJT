using DataAccessLayer.Base;
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
    public class TrainingPlanRepository : GenericRepository<TrainingPlan>, ITrainingPlanRepository
    {
        public TrainingPlanRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<TrainingPlan> GetTrainingPLanByIdAndDeleteIsFalse(int id)
        {
            var tp = await _context.TrainingPlans.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
            return tp;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanList()
        {
            var list = await _context.TrainingPlans.Where(u => u.IsDeleted == false).ToListAsync();
            return list;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanListByOwnerId(int id)
        {
            var list = await _context.UserTrainingPlans
                .Where(u => u.UserId == id && u.IsOwner == true)
                .Select(u => u.TrainingPlan)
                .ToListAsync();
            return list;
        }
    }
}
