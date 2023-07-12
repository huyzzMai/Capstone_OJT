﻿using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.TrainingPLanRequest;
using BusinessLayer.Models.ResponseModel;
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
        Task<TrainingPlanResponse> GetTrainingPlanForAllRole(int userId, int id);

        Task<BasePagingViewModel<TrainingPlanResponse>> GetTrainingPlanList(PagingRequestModel paging);

        Task<BasePagingViewModel<TrainingPlanResponse>> GetTrainingPlanListByOwner(int id, PagingRequestModel paging);

        Task CreateTrainingPlan(int userId, CreateTrainingPlanRequest request);

        Task UpdateTrainingPlan(int userId, int planId, UpdateTrainingPlanRequest request);

        Task DeactivateTrainingPlan(int userId, int planId);

        Task OpenDeactivatedTrainingPlan(int userId, int planId);

        Task AcceptTrainingPlan(int id);

        Task DenyTrainingPlan(int id);

        Task CreateTrainingPlanDetailForExistingTrainingPlan(int userId, int planId, CreateTrainingPlanDetailRequest request);

        Task UpdateTrainingPlanDetail(int userId, int detailId, CreateTrainingPlanDetailRequest request);

        Task AssignTraineeToTrainingPlan(int trainerId, int traineeId, int planId);
    }
}
