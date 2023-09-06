using BusinessLayer.Payload.RequestModel.CriteriaRequest;
using BusinessLayer.Payload.ResponseModel.CriteriaResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IUserCriteriaService
    {
        Task<List<UserCriteriaResponse>> GetUserCriteriaToGrade(int tranerId, int ojtBatchId);

        Task<List<UserCriteriaResponse>> GetCurrentUserCriteria(int tranerId, int ojtBatchId);
        Task UpdatePoints(int trainerId, List<UpdateCriteriaRequest> requests);
    }
}
