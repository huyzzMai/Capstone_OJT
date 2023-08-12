using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Models.ResponseModel.CriteriaResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IUserCriteriaService
    {
        Task<List<UserCriteriaResponse>> GetUserCriteria(int tranerId, int ojtBatchId);

        Task UpdatePoints(int trainerId, List<UpdateCriteriaRequest> requests);
    }
}
