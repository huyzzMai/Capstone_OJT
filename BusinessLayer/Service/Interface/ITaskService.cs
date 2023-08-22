using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITaskService
    {
        Task<BasePagingViewModel<TraineeTaskResponse>> GetAllTaskOfTrainee(int userId, PagingRequestModel paging);

        //Task<IEnumerable<TraineeTaskResponse>> GetListUnFinishTaskOfTrainee(int userId);

        Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskAccomplished(int userId, PagingRequestModel paging);

        Task<BasePagingViewModel<TaskAccomplishedResponse>> GetListTaskPendingOfTrainee(int trainerId, int traineeId, PagingRequestModel paging);  

        Task CreateFinishTask(string taskId);

        Task AcceptTraineeTask(int trainerId, int taskId);

        Task RejectTraineeTask(int trainerId, int taskId);

        Task<TaskCounterResponse> CountTaskOfTrainee(int traineeId);

        Task CreateBoardWebhook(int userId);
    }
}
