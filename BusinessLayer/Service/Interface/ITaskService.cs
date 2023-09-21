using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.TaskResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITaskService
    {
        Task<BasePagingViewModel<TraineeTaskResponse>> GetAllTaskOfTrainee(int userId, PagingRequestModel paging);

        Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskAccomplished(int userId, PagingRequestModel paging);

        Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskOfTrainee(int trainerId, int traineeId, PagingRequestModel paging, int? status);  

        Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListAllTaskOfTrainees(int trainerId, PagingRequestModel paging, int? status);

        Task<TaskAccomplishedResponse> GetTaskAccomplishedById(int taskId, int trainerId); 

        Task<List<WebhookBoardsResponse>> GetListOpenBoard(int userId);

        Task<BasePagingViewModel<TaskAccomplishedWithTraineeInfoResponse>> GetListTaskAccomplishOfBoard(string boardId, PagingRequestModel paging, int? status);

        Task CreateFinishTask(string taskId);

        Task AcceptTraineeTask(int trainerId, int taskId);

        Task RejectTraineeTask(int trainerId, int taskId);

        Task<TaskCounterResponse> CountTaskOfTrainee(int traineeId);

        Task<List<WebhookBoardsResponse>> CreateBoardWebhook(int userId);
    }
}
