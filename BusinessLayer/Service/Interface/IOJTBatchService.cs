using BusinessLayer.Models.RequestModel.OjtBatchRequest;
using BusinessLayer.Models.ResponseModel.OJTBatchResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IOJTBatchService
    {
        Task CreateOjtBatch(CreateOjtBatchRequest request);

        Task<OjtBatchDetailResponse> GetDetailOjtBatch(int batchId);

        Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtList();

        Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtListbyUniversityId(int id);
    }
}
