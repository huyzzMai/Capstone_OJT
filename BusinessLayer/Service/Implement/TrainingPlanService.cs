using BusinessLayer.Models.RequestModel.TrainingPLanRequest;
using BusinessLayer.Models.ResponseModel.TrainingPlanResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainingPlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrainingPlanResponse>> GetTrainingPlanList()
        {
            var tplans = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanList();
            IEnumerable<TrainingPlanResponse> res = tplans.Select(
                tplan =>
                {
                    return new TrainingPlanResponse()
                    {
                        Id = tplan.Id,
                        Name = tplan.Name,
                        Status = tplan.Status
                    };
                }
                ).ToList();
            return res;
        }

        public async Task<IEnumerable<TrainingPlanResponse>> GetTrainingPlanListByOwner(int id)
        {
            var tplans = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanListByOwnerId(id);
            IEnumerable<TrainingPlanResponse> res = tplans.Select(
                tplan =>
                {
                    return new TrainingPlanResponse()
                    {
                        Id = tplan.Id,
                        Name = tplan.Name
                    };
                }
                ).ToList();
            return res;
        }

        public async Task CreateTrainingPlan(int userId, CreateTrainingPlanRequest request)
        {
            //foreach (var detail in request.Details)
            //{

            //}

            TrainingPlan tp = new()
            {
                Name = request.Name,
                Status = CommonEnums.TRAINING_PLAN_STATUS.PENDING,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.TrainingPlanRepository.Add(tp);

            UserTrainingPlan utp = new()
            {
                UserId = userId,
                TrainingPlanId = tp.Id,
                IsOwner = true
            };
            await _unitOfWork.UserTrainingPlanRepository.Add(utp);
        }

        public async Task AcceptTrainingPlan(int id)
        {
            try
            {
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndDeleteIsFalse(id);
                if (tp == null)
                {
                    throw new Exception("Training plan not found !");
                }
                else if (tp.Status != CommonEnums.TRAINING_PLAN_STATUS.PENDING)
                {
                    throw new Exception("Training plan is not on Pending status !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.ACCEPTED;
                tp.UpdatedAt = DateTime.Now;
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DenyTrainingPlan(int id)
        {
            try
            {
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndDeleteIsFalse(id);
                if (tp == null)
                {
                    throw new Exception("Training plan not found !");
                }
                else if (tp.Status != CommonEnums.TRAINING_PLAN_STATUS.PENDING)
                {
                    throw new Exception("Training plan is not on Pending status !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.DENIED;
                tp.UpdatedAt = DateTime.Now;
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
