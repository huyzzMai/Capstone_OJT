using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.OjtBatchRequest;
using BusinessLayer.Models.ResponseModel;
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

        Task UpdateOjtBatch(int id,UpdateOjtBatchRequest request);

        Task DeleteOjtBatch(int id);

        Task<BasePagingViewModel<ValidOJTBatchResponse>> GetValidOJtList(PagingRequestModel paging);

        Task<BasePagingViewModel<ValidOJTBatchResponse>> GetValidOJtListbyUniversityId(int id, PagingRequestModel paging);

        Task<List<ListActiveOjtbatchforTrainer>> getListNotGradePointOjtbatch(int trainerId);
    }
}
