using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Models.ResponseModel.TrainingPlanResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TrelloDotNet;

namespace BusinessLayer.Service.Implement
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IConfiguration _configuration;

        public TaskService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<BasePagingViewModel<TraineeTaskResponse>> GetAllTaskOfTrainee(int userId, PagingRequestModel paging)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }
                var trelloUserId = user.TrelloId;
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
                
                var cards = await client.GetCardsForMemberAsync(trelloUserId);

                List<TraineeTaskResponse> res = new List<TraineeTaskResponse>(); 

                foreach ( var card in cards)
                {
                    DateTimeOffset og = card.Start ?? default(DateTimeOffset);
                    card.Start = og.ToLocalTime();
                    
                    DateTimeOffset og2 = card.Due ?? default(DateTimeOffset);
                    var dueCheck = og2.ToLocalTime();

                    TraineeTaskResponse task = new TraineeTaskResponse();   
                    task.Id = card.Id;
                    task.Name = card.Name;
                    task.Description = card.Description;
                    task.StartTime = card.Start;
                    task.EndTime = dueCheck;
                    if (card.DueComplete == false)
                    {
                        if (dueCheck.CompareTo(DateTimeOffset.Now) < 0)
                        {
                            task.Status = CommonEnums.TRAINEE_TASK_STATUS.OVERDUE;
                        }
                        else
                        {
                            task.Status = CommonEnums.TRAINEE_TASK_STATUS.IN_PROCESS;
                        }
                    }
                    else
                    {
                        task.Status = CommonEnums.TRAINEE_TASK_STATUS.FINISHED;
                    }

                    res.Add(task);
                }
                
                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TraineeTaskResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<IEnumerable<TraineeTaskResponse>> GetListUnFinishTaskOfTrainee(int userId)
        //{
        //    try
        //    {
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskAccomplished(int userId, PagingRequestModel paging)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                var tasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedOfTrainee(userId);

                List<TaskAccomplishedResponse> res = tasks.Select(
                task =>
                {
                    return new TaskAccomplishedResponse()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Description = task.Description,
                        StartTime = task.StartDate,
                        EndTime = task.DueDate,
                        FinishTime = task.AccomplishDate,
                        ProcessStatus = task.Status
                    };
                }
                ).ToList();

                int totalItem = res.Count;

                var result = new BasePagingViewModel<TaskAccomplishedResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateFinishTask(int userId, string taskId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
                var card = await client.GetCardAsync(taskId);

                card.DueComplete = true;
                var updatedCard = await client.UpdateCardAsync(card);

                TaskAccomplished ta = new()
                {
                    Id = taskId,
                    Name = card.Name,
                    Description = card.Description,
                    StartDate = card.Start,
                    DueDate = card.Due,
                    AccomplishDate = DateTimeOffset.Now,
                    Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.PENDING,
                    UserId = userId
                };

                await _unitOfWork.TaskRepository.Add(ta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AcceptTraineeTask(int trainerId, string taskId)
        {
            try
            {
                var task = await _unitOfWork.TaskRepository.GastTaskByIdAndStatusPending(taskId);
                if (task == null)
                {
                    throw new Exception("Task not found or task not pending!");
                }
                if (trainerId != task.User.UserReferenceId)
                {
                    throw new Exception("Not your trainee's task!");
                }

                task.Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.DONE;
                await _unitOfWork.TaskRepository.Update(task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RejectTraineeTask(int trainerId, string taskId)
        {
            try
            {
                var task = await _unitOfWork.TaskRepository.GastTaskByIdAndStatusPending(taskId);
                if (task == null)
                {
                    throw new Exception("Task not found or task not pending!");
                }
                if (trainerId != task.User.UserReferenceId)
                {
                    throw new Exception("Not your trainee's task!");
                }

                task.Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.FAILED;
                await _unitOfWork.TaskRepository.Update(task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
