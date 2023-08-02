using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.TrainingPLanRequest;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.TrainingPlanResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Utilities;

namespace BusinessLayer.Service.Implement
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainingPlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TrainingPlanResponse> GetTrainingPlanForAllRole(int userId, int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);

                if (user.Role == CommonEnums.ROLE.TRAINEE)
                {
                    var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanById(userId, id);
                    if (check == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Training plan not found or You are not assigned to this training plan !");
                    }
                    var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndStatusActive(id);
                    if (tp == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found!");
                    }

                    List<TrainingPlanDetail> l = tp.TrainingPlanDetails.ToList();
                    var detailList = l
                                     .Where(u => u.Status == CommonEnums.TRAINING_PLAN_DETAIL_STATUS.ACTIVE)
                                     .Select(
                                     detail =>
                                     {
                                         return new TrainingPlanDetailResponse()
                                         {
                                             Id = detail.Id,
                                             Name = detail.Name,
                                             Description = detail.Description,
                                             StartTime = detail.StartTime,
                                             EndTime = detail.EndTime,
                                             IsEvaluativeTask = detail.IsEvaluativeTask
                                         };
                                     }
                                     ).ToList()
                                     ;

                    var res = new TrainingPlanResponse()
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        Status = tp.Status,
                        Details = detailList
                    };
                    return res;
                } 
                else if (user.Role == CommonEnums.ROLE.MANAGER)
                {
                    var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(id);
                    if (tp == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found!");
                    }

                    List<TrainingPlanDetail> l = tp.TrainingPlanDetails.ToList();
                    var detailList = l
                                     .Where(u => u.Status == CommonEnums.TRAINING_PLAN_DETAIL_STATUS.ACTIVE)
                                     .Select(
                                     detail =>
                                     {
                                         return new TrainingPlanDetailResponse()
                                         {
                                             Id = detail.Id,
                                             Name = detail.Name,
                                             Description = detail.Description,
                                             StartTime = detail.StartTime,
                                             EndTime = detail.EndTime,
                                             IsEvaluativeTask = detail.IsEvaluativeTask
                                         };
                                     }
                                     ).ToList()
                                     ;
                    var res = new TrainingPlanResponse()
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        Status = tp.Status,
                        Details = detailList
                    };
                    return res;
                }
                else
                {
                    var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, id);
                    if (check == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found or You not the owner of this training plan !");
                    }
                    var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(id);
                    if (tp == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found!");
                    }

                    List<TrainingPlanDetail> l = tp.TrainingPlanDetails.ToList();
                    var detailList = l
                                     .Where(u => u.Status != CommonEnums.TRAINING_PLAN_DETAIL_STATUS.DELETED)
                                     .Select(
                                     detail =>
                                     {
                                         return new TrainingPlanDetailResponse()
                                         {
                                             Id = detail.Id,
                                             Name = detail.Name,
                                             Description = detail.Description,
                                             StartTime = detail.StartTime,
                                             EndTime = detail.EndTime,
                                             IsEvaluativeTask = detail.IsEvaluativeTask,
                                             Status = detail.Status,
                                         };
                                     }
                                     ).ToList()
                                     ;
                    var res = new TrainingPlanResponse()
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        Status = tp.Status,
                        Details = detailList
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BasePagingViewModel<TrainingPlanResponse>> GetTrainingPlanList(PagingRequestModel paging, string keyword)
        {
            try
            {
                List<TrainingPlan> tplans = new List<TrainingPlan>();   
                if (keyword == null)
                {
                    tplans = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanList();
                }
                else
                {
                    keyword = keyword.ToLower();
                    tplans = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanListSearchKeyword(keyword);
                }

                List<TrainingPlanResponse> res = tplans.Select(
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

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TrainingPlanResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BasePagingViewModel<TrainingPlanResponse>> GetTrainingPlanListByOwner(int id, PagingRequestModel paging)
        {
            var tplans = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanListByOwnerId(id);
            List<TrainingPlanResponse> res = tplans.Select(
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

            int totalItem = res.Count;

            res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

            var result = new BasePagingViewModel<TrainingPlanResponse>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = res
            };
            return result;
        }

        public async Task AssignTraineeToTrainingPlan(int trainerId, int traineeId, int planId)
        {
            try
            {
                var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                if (trainee == null || trainee.UserReferenceId != trainerId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Not found trainee or this is not your assiged trainee!");
                }

                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(trainerId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or you not the owner of this training plan!");
                }

                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndStatusActive(planId);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Not found training plan!");
                }

                UserTrainingPlan utp = new()
                {
                    UserId = traineeId,
                    TrainingPlanId = planId,
                    IsOwner = false
                };
                await _unitOfWork.UserTrainingPlanRepository.Add(utp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateTrainingPlan(int userId, CreateTrainingPlanRequest request)
        {
            try
            {
                TrainingPlan tp = new()
                {
                    Name = request.Name,
                    Status = CommonEnums.TRAINING_PLAN_STATUS.PENDING,
                    CreatedAt = DateTime.UtcNow.AddHours(7)
                };

                ICollection<TrainingPlanDetail> re = new List<TrainingPlanDetail>();
                foreach (var detail in request.Details)
                {
                    TrainingPlanDetail tpd = new()
                    {
                        Name = detail.Name,
                        Description = detail.Description,
                        StartTime = detail.StartTime,
                        EndTime = detail.EndTime,
                        IsEvaluativeTask = detail.IsEvaluativeTask,
                        Status = CommonEnums.TRAINING_PLAN_DETAIL_STATUS.ACTIVE,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        TrainingPlanId = tp.Id
                    };
                    re.Add(tpd);
                }

                tp.TrainingPlanDetails = re;

                await _unitOfWork.TrainingPlanRepository.Add(tp);

                UserTrainingPlan utp = new()
                {
                    UserId = userId,
                    TrainingPlanId = tp.Id,
                    IsOwner = true
                };
                await _unitOfWork.UserTrainingPlanRepository.Add(utp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTrainingPlan(int userId, int planId, UpdateTrainingPlanRequest request)
        {
            try
            {
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or You not the owner of this training plan !");
                }
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(planId);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }

                if (request.Name != null)
                {
                    tp.Name = request.Name;
                }
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanRepository.Update(tp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeactivateTrainingPlan(int userId, int planId)
        {
            try
            {
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or You not the owner of this training plan !");
                }
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(planId);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }
                if (tp.Status == CommonEnums.TRAINING_PLAN_STATUS.CLOSED)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Cannot deactivate closed training plan !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.CLOSED;
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task OpenDeactivatedTrainingPlan(int userId, int planId)
        {
            try
            {
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or You not the owner of this training plan !");
                }
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(planId);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }
                if (tp.Status == CommonEnums.TRAINING_PLAN_STATUS.ACTIVE)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Cannot re-open active training plan !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.ACTIVE;
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateTrainingPlanDetailForExistingTrainingPlan(int userId, int planId, CreateTrainingPlanDetailRequest request)
        {
            try
            {
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or You not the owner of this training plan !");
                }
                var tpcheck = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(planId);
                if (tpcheck == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }
                TrainingPlanDetail tpd = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    IsEvaluativeTask = request.IsEvaluativeTask,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.TRAINING_PLAN_DETAIL_STATUS.ACTIVE,
                    TrainingPlanId = planId
                };
                await _unitOfWork.TrainingPlanDetailRepository.Add(tpd);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTrainingPlanDetail(int userId, int detailId, CreateTrainingPlanDetailRequest request) 
        {
            try
            {
                var detail = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanDetailByIdAndNotDeleted(detailId);
                if (detail == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan detail not found !");
                }
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(userId, detail.TrainingPlanId ?? default(int));
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "You are not the owner of this training plan detail !");
                }

                if (request.Name != null)
                {
                    detail.Name = request.Name;
                }
                if (request.Description != null)
                {
                    detail.Description = request.Description;
                }
                if (request.StartTime != null)
                {
                    detail.StartTime = request.StartTime;
                }
                if (request.EndTime != null)
                {
                    detail.EndTime = request.EndTime;
                }
                if (request.IsEvaluativeTask != null)
                {
                    detail.IsEvaluativeTask = request.IsEvaluativeTask;
                }

                detail.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanDetailRepository.Update(detail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task AcceptTrainingPlan(int id)
        {
            try
            {
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(id);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }
                else if (tp.Status != CommonEnums.TRAINING_PLAN_STATUS.PENDING)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan is not on Pending status !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.ACTIVE;
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
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
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(id);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }
                else if (tp.Status != CommonEnums.TRAINING_PLAN_STATUS.PENDING)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan is not on Pending status !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.DENIED;
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTrainingPlan(int planId, int trainerId)
        {
            try
            {
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(trainerId, planId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Training plan not found or You not the owner of this training plan !");
                }
                var tp = await _unitOfWork.TrainingPlanRepository.GetTrainingPLanByIdAndNotDeleted(planId);
                if (tp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan not found !");
                }

                tp.Status = CommonEnums.TRAINING_PLAN_STATUS.DELETED;
                tp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TrainingPlanRepository.Update(tp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTrainingPlanDetail(int detailId, int trainerId)
        {
            try
            {
                var detail = await _unitOfWork.TrainingPlanRepository.GetTrainingPlanDetailByIdAndNotDeleted(detailId);
                if (detail == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Training plan detail not found !");
                }
                var check = await _unitOfWork.TrainingPlanRepository.GetUserTrainingPlanByIdAndIsOwner(trainerId, detail.TrainingPlanId ?? default(int));
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "You are not the owner of this training plan detail !");
                }

                detail.Status = CommonEnums.TRAINING_PLAN_DETAIL_STATUS.DELETED;
                detail.UpdatedAt = DateTime.UtcNow.AddHours(7);    
                await _unitOfWork.TrainingPlanDetailRepository.Update(detail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
