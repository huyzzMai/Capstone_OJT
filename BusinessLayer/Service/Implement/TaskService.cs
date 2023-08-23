using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrelloDotNet;
using TrelloDotNet.Model.Webhook;

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
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                if (user.TrelloId == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User have not update TrelloId!");
                }
                var trelloUserId = user.TrelloId;
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var cards = await client.GetCardsForMemberAsync(trelloUserId);

                List<TraineeTaskResponse> res = new List<TraineeTaskResponse>();

                foreach (var card in cards)
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
                        if (dueCheck.CompareTo(DateTimeOffset.UtcNow.AddHours(7)) < 0)
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
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }

                var tasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedOfTrainee(userId);

                List<TaskAccomplishedResponse> res = tasks.Select(
                task =>
                {
                    return new TaskAccomplishedResponse()
                    {
                        Id = task.Id,
                        TrelloTaskId = task.TrelloTaskId,
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

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

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

        public async Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskPendingOfTrainee(int trainerId, int traineeId, PagingRequestModel paging)
        {
            try
            {
                var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                if (trainee == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainee not found!");
                }
                if (trainee.UserReferenceId != trainerId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "This is not your trainee!");
                }

                var tasks = await _unitOfWork.TaskRepository.GetListTaskPendingOfTrainee(traineeId);

                List<TaskAccomplishedResponse> res = tasks.Select(
                task =>
                {
                    return new TaskAccomplishedResponse()
                    {
                        Id = task.Id,
                        TrelloTaskId = task.TrelloTaskId,
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

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

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

        public async Task CreateFinishTask(string taskId)
        {
            try
            {
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
                var card = await client.GetCardAsync(taskId);
                var memberIds = card.MemberIds;
                if (memberIds == null || memberIds.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "There is no user for this Task!");
                }
                var memId = memberIds.FirstOrDefault();

                var trainee = await _unitOfWork.UserRepository.GetUserByTrelloIdAndStatusActive(memId);
                if (trainee == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found or User have not update TrelloId!");
                }
                var matchingTask = await _unitOfWork.TaskRepository.GetMatchingTask(taskId, trainee.Id);
                if (matchingTask != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "This task had been submit to the system!");
                }

                TaskAccomplished ta = new()
                {
                    TrelloTaskId = taskId,
                    Name = card.Name,
                    Description = card.Description,
                    StartDate = card.Start,
                    DueDate = card.Due,
                    AccomplishDate = DateTimeOffset.UtcNow.AddHours(7),
                    Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.PENDING,
                    UserId = trainee.Id
                };

                await _unitOfWork.TaskRepository.Add(ta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AcceptTraineeTask(int trainerId, int taskId)
        {
            try
            {
                var task = await _unitOfWork.TaskRepository.GastTaskByIdAndStatusPending(taskId);
                if (task == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Task not found or task not pending!");
                }
                if (trainerId != task.User.UserReferenceId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Not your trainee's task!");
                }

                task.Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.DONE;
                await _unitOfWork.TaskRepository.Update(task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RejectTraineeTask(int trainerId, int taskId)
        {
            try
            {
                var task = await _unitOfWork.TaskRepository.GastTaskByIdAndStatusPending(taskId);
                if (task == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Task not found or task not pending!");
                }
                if (trainerId != task.User.UserReferenceId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Not your trainee's task!");
                }

                task.Status = CommonEnums.ACCOMPLISHED_TASK_STATUS.FAILED;
                await _unitOfWork.TaskRepository.Update(task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TaskCounterResponse> CountTaskOfTrainee(int traineeId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                if (user.TrelloId == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User have not update TrelloId!");
                }
                var trelloUserId = user.TrelloId;
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var cards = await client.GetCardsForMemberAsync(trelloUserId);
                int totalTask = cards.Count;

                var pendingTask = await _unitOfWork.TaskRepository.GetListTaskPendingOfTrainee(traineeId);
                var totalPeningTask = pendingTask.Count;  

                var doneTasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedDoneOfTrainee(traineeId);
                int totalDoneTask = doneTasks.Count;

                var failTasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedFailedOfTrainee(traineeId);
                int totalFailTask = failTasks.Count;  

                int totalOverdueTask = totalTask - totalDoneTask - totalFailTask - totalPeningTask;

                TaskCounterResponse result = new()
                {
                    TotalTask = totalTask,
                    TaskComplete = totalDoneTask,
                    TaskOverdue = totalOverdueTask,
                    TaskFail = totalFailTask
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateBoardWebhook(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                var trelloUserId = user.TrelloId;
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var boards = await client.GetBoardsForMemberAsync(trelloUserId);
                if (boards == null || boards.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "You have not create any Board!");
                }

                var webhooks = await client.GetWebhooksForCurrentTokenAsync();

                foreach (var board in boards)
                {
                    var check = webhooks.FirstOrDefault(u => u.IdOfTypeYouWishToMonitor == board.Id);
                    if (check == null)
                    {
                        string description = "Webhook of Board : " + board.Name;
                        var newWebhook = new Webhook(description, _configuration["TrelloWorkspace:URLCallBack"], board.Id);
                        var addedWebhook = await client.AddWebhookAsync(newWebhook);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
