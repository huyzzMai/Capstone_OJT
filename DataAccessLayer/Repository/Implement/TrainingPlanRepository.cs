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
    public class TrainingPlanRepository : GenericRepository<TrainingPlan>, ITrainingPlanRepository
    {
        public TrainingPlanRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<TrainingPlan> GetTrainingPLanById(int id)
        {
            var tp = await _context.TrainingPlans
                     .Where(u => u.Id == id)
                     .Include(u => u.TrainingPlanDetails)
                     .FirstOrDefaultAsync();
            return tp;
        }

        public async Task<User> GetOwnerByTrainingPlanId(int id)
        {
            var utp = await _context.UserTrainingPlans
                .Where(u => u.TrainingPlanId == id && u.IsOwner == true)
                .Select(u => u.User)
                .FirstOrDefaultAsync();
            return utp;
        }

        public async Task<TrainingPlan> GetTrainingPlanByTraineeIdAndStatusActive(int traineeId)
        {
            var tp = await _context.UserTrainingPlans
                     .Where(u => u.TrainingPlanId == traineeId)
                     .Include(u => u.TrainingPlan.TrainingPlanDetails)
                     .Select(u => u.TrainingPlan)
                     .Where(u => u.Status == CommonEnums.TRAINING_PLAN_STATUS.ACTIVE)
                     .FirstOrDefaultAsync();
            return tp;
        }

        public async Task<TrainingPlan> GetTrainingPLanByIdAndStatusActive(int id)
        {
            var tp = await _context.TrainingPlans
                     .Where(u => u.Id == id && u.Status == CommonEnums.TRAINING_PLAN_STATUS.ACTIVE)
                     .Include(u => u.TrainingPlanDetails)
                     .FirstOrDefaultAsync();
            return tp;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanList()
        {
            var list = await _context.TrainingPlans
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return list;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanListSearchKeyword(string keyword)
        {
            var list = await _context.TrainingPlans
                .Where(u => u.Name.ToLower().Contains(keyword))
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return list;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanListByOwnerId(int id)
        {
            var list = await _context.UserTrainingPlans
                .Where(u => u.UserId == id && u.IsOwner == true)
                .Select(u => u.TrainingPlan)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return list;
        }

        public async Task<List<TrainingPlan>> GetTrainingPlanListByOwnerSearchKeyword(int id, string keyword)
        {
            var list = await _context.UserTrainingPlans
                .Where(u => u.UserId == id && u.IsOwner == true)
                .Select(u => u.TrainingPlan)
                .Where(u => u.Name.ToLower().Contains(keyword))
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return list;
        }

        public async Task<UserTrainingPlan> GetUserTrainingPlanByIdAndIsOwner(int userId, int planId)
        {
            var u = await _context.UserTrainingPlans
                .Where(u => u.UserId == userId && u.TrainingPlanId == planId && u.IsOwner == true)
                //.Include("TrainingPlan")
                .FirstOrDefaultAsync();
            return u;
        }

        public async Task<UserTrainingPlan> GetUserTrainingPlanById(int userId, int planId)
        {
            var u = await _context.UserTrainingPlans
                .Where(u => u.UserId == userId && u.TrainingPlanId == planId)
                .FirstOrDefaultAsync();
            return u;
        }

        public async Task<TrainingPlanDetail> GetTrainingPlanDetailById(int id)
        {
            var detail = await _context.TrainingPlanDetails
                         .Where(u => u.Id == id)
                         .FirstOrDefaultAsync();
            return detail;  
        }
    }
}
