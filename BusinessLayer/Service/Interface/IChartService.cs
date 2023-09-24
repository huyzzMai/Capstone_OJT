using BusinessLayer.Payload.ResponseModel.ChartResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IChartService
    {
        Task<BatchAndTraineeResponse> getBatchAndTrainee(int year);

        Task<List<ChartCircleResponse>> getTrainerWithMostTrainees(int number);

        Task<List<ChartCircleResponse>> getTraineeByPosition();

        Task<List<TopSkillTraineeResponse>> GetTopSkillByTrainee(int userId);

        Task<List<TopTaskTraineeResponse>> GetTopTraineeWithMostApprovedTask(int userId); 
    }
}
