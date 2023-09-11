using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.OjtBatchRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.OJTBatchResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
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

        Task<List<ListActiveOjtbatchforTrainer>> getListGradePointOjtbatch(int trainerId);
        Task<BasePagingViewModel<ListOjtExport>> getListOjtbatchExportStatus(PagingRequestModel paging, string searchTerm, string Status);
    }
}
