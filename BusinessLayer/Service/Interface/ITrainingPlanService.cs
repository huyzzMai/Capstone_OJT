using BusinessLayer.Models.RequestModel.TrainingPLanRequest;
using BusinessLayer.Models.ResponseModel.TrainingPlanResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITrainingPlanService
    {
        Task<IEnumerable<TrainingPlanResponse>> GetTrainingPlanList();

        Task<IEnumerable<TrainingPlanResponse>> GetTrainingPlanListByOwner(int id);

        Task CreateTrainingPlan(int userId, CreateTrainingPlanRequest request);

        Task AcceptTrainingPlan(int id);

        Task DenyTrainingPlan(int id);  
    }
}
