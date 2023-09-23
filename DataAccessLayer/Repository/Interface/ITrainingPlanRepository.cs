using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ITrainingPlanRepository : IGenericRepository<TrainingPlan>
    {

        Task<TrainingPlan> GetTrainingPLanById(int id);

        Task<User> GetOwnerByTrainingPlanId(int id);

        Task<TrainingPlan> GetTrainingPlanByTraineeIdAndStatusActive(int traineeId);    

        Task<TrainingPlan> GetTrainingPLanByIdAndStatusActive(int id);

        Task<List<TrainingPlan>> GetTrainingPlanList();

        Task<List<TrainingPlan>> GetTrainingPlanListSearchKeyword(string keyword);

        Task<List<TrainingPlan>> GetTrainingPlanListByOwnerId(int id);

        Task<List<TrainingPlan>> GetTrainingPlanListByOwnerSearchKeyword(int id, string keyword);

        Task<UserTrainingPlan> GetUserTrainingPlanByIdAndIsOwner(int userId, int planId);

        Task<UserTrainingPlan> GetUserTrainingPlanById(int userId, int planId);

        Task<TrainingPlanDetail> GetTrainingPlanDetailById(int id);
    }
}
