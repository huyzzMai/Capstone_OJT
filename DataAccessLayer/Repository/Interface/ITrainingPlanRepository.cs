﻿using DataAccessLayer.Interface;
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

        Task<TrainingPlan> GetTrainingPLanByIdAndNotDeleted(int id);

        Task<TrainingPlan> GetTrainingPLanByIdAndStatusActive(int id);

        Task<List<TrainingPlan>> GetTrainingPlanList();

        Task<List<TrainingPlan>> GetTrainingPlanListByOwnerId(int id);

        Task<UserTrainingPlan> GetUserTrainingPlanByIdAndIsOwner(int userId, int planId);

        Task<UserTrainingPlan> GetUserTrainingPlanById(int userId, int planId);

        Task<TrainingPlanDetail> GetTrainingPlanDetailByIdAndNotDeleted(int id);
    }
}