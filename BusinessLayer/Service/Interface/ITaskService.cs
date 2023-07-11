using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Models.ResponseModel.TrainingPlanResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITaskService
    {
        Task<IEnumerable<TraineeTaskResponse>> GetAllTaskOfTrainee(int userId);

        //Task<IEnumerable<TraineeTaskResponse>> GetListUnFinishTaskOfTrainee(int userId);

        Task<IEnumerable<TraineeTaskResponse>> GetListTaskAccomplished(int userId);

        Task UpdateFinishTask(int userId, string taskId);  
    }
}
