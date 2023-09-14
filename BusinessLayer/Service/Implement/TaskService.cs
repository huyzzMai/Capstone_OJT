using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.TaskResponse;
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

        public async Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListAllTaskOfTrainees(int trainerId, PagingRequestModel paging, int? status)
        {
            try
            {
                var trainees = await _unitOfWork.UserRepository.GetTraineeListByTrainerId(trainerId);

                List<TaskAccomplished> tasks = new();

                foreach(var trainee in trainees)
                {
                    var listTask = await _unitOfWork.TaskRepository.GetListTaskAccomplishedOfTrainee(trainee.Id);
                    tasks.AddRange(listTask);   
                }
                if (status != null)
                {
                    tasks = tasks.Where(task => task.Status == status).ToList();
                }

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

        public async Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskOfTrainee(int trainerId, int traineeId, PagingRequestModel paging, int? status)
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

                var tasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedOfTrainee(traineeId);

                if(status != null)
                {
                    tasks = tasks.Where(task => task.Status == status).ToList(); 
                }

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

        public async Task<TaskAccomplishedResponse> GetTaskAccomplishedById(int taskId, int trainerId)
        {
            try
            {
                var task = await _unitOfWork.TaskRepository.GetTaskAccomplishedById(taskId);
                if (task == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Task Accomplished not found!");
                }
                if (task.User.UserReferenceId == null || task.User.UserReferenceId != trainerId)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "This task not belong to your trainee!");
                }
                TaskAccomplishedResponse res = new()
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
                return res;
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

        public async Task<List<WebhookBoardsResponse>> CreateBoardWebhook(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                var trelloUserId = user.TrelloId;
                if (trelloUserId == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User does not have a Trello Account!");
                }
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var boards = await client.GetBoardsForMemberAsync(trelloUserId);
                boards = boards.Where(b => b.Closed ==false).ToList();  

                if (boards == null || boards.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "You have not create any Board!");
                }
                var response = new List<WebhookBoardsResponse>();
                var webhooks = await client.GetWebhooksForCurrentTokenAsync();

                foreach (var board in boards)
                {
                    var check = webhooks.FirstOrDefault(u => u.IdOfTypeYouWishToMonitor == board.Id);
                    if (check == null)
                    {
                        string description = "Webhook of Board : " + board.Name;
                        var newWebhook = new Webhook(description, _configuration["TrelloWorkspace:URLCallBack"], board.Id);
                        try
                        {
                            var addedWebhook = await client.AddWebhookAsync(newWebhook);
                        }
                        catch (Exception webhookEx)
                        {
                            // Handle exception specific to adding webhook
                            Console.WriteLine($"An error occurred while adding a webhook: {webhookEx.Message}");
                            throw new ApiException(CommonEnums.CLIENT_ERROR.REQUEST_TIMEOUT,"Trello server took too much time to process the request. Please try to reattempt to send this request!");
                        }
                    }
                    WebhookBoardsResponse a = new()
                    {
                        BoardTrelloId = board.Id,
                        BoardName = board.Name,
                        BoardURL = board.Url,
                    };
                    response.Add(a);    
                }
                return response;    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<WebhookBoardsResponse>> GetListOpenBoard(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                var trelloUserId = user.TrelloId;
                if (trelloUserId == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User does not have a Trello Account!");
                }
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var boards = await client.GetBoardsForMemberAsync(trelloUserId);
                boards = boards.Where(b => b.Closed == false).ToList();

                if (boards == null || boards.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "You have not create any Board!");
                }
                var response = new List<WebhookBoardsResponse>();
                var webhooks = await client.GetWebhooksForCurrentTokenAsync();

                foreach (var board in boards)
                {
                    var check = webhooks.FirstOrDefault(u => u.IdOfTypeYouWishToMonitor == board.Id);
                    if (check == null)
                    {
                        string description = "Webhook of Board : " + board.Name;
                        var newWebhook = new Webhook(description, _configuration["TrelloWorkspace:URLCallBack"], board.Id);
                        try
                        {
                            var addedWebhook = await client.AddWebhookAsync(newWebhook);
                        }
                        catch (Exception webhookEx)
                        {
                            // Handle exception specific to adding webhook
                            Console.WriteLine($"An error occurred while adding a webhook: {webhookEx.Message}");
                            throw new ApiException(CommonEnums.CLIENT_ERROR.REQUEST_TIMEOUT, "Trello server took too much time to process the request. Please try to reattempt to send this request!");
                        }
                    }
                    WebhookBoardsResponse a = new()
                    {
                        BoardTrelloId = board.Id,
                        BoardName = board.Name,
                        BoardURL = board.Url,
                    };
                    response.Add(a);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BasePagingViewModel<TaskAccomplishedWithTraineeInfoResponse>> GetListTaskAccomplishOfBoard(string boardId, PagingRequestModel paging)
        {
            try
            {
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
                var cards = await client.GetCardsOnBoardAsync(boardId);
                if (cards == null || cards.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "There is no task in this Board!");
                }
                List<TaskAccomplished> tasks = new();
                foreach (var card in cards)
                {
                    var task = await _unitOfWork.TaskRepository.GetTaskAccomplishedByTrelloTaskId(card.Id); 
                    if (task == null)
                    {
                        continue;
                    }
                    tasks.Add(task);
                }
                List<TaskAccomplishedWithTraineeInfoResponse> res = tasks.Select(
                task =>
                {
                    return new TaskAccomplishedWithTraineeInfoResponse()
                    {
                        Id = task.Id,
                        TrelloTaskId = task.TrelloTaskId,
                        Name = task.Name,
                        Description = task.Description,
                        StartTime = task.StartDate,
                        EndTime = task.DueDate,
                        FinishTime = task.AccomplishDate,
                        ProcessStatus = task.Status,
                        TraineeFirstName = task.User.FirstName,
                        TraineeLastName = task.User.LastName,
                        TraineeRollNumber = task.User.RollNumber
                    };
                }
                ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

                var result = new BasePagingViewModel<TaskAccomplishedWithTraineeInfoResponse>()
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
    }
}
