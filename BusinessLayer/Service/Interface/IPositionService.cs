using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Payload.RequestModel.PositionRequest;
using BusinessLayer.Payload.ResponseModel.PositionResponse;

namespace BusinessLayer.Service.Interface
{
    public interface IPositionService
    {
        Task CreatePosition(CreatePositionRequest request);

        Task UpdatePositon(int id, UpdatePositionRequest request);

        Task DeletePosition(int id);

        Task<BasePagingViewModel<ListPostionResponse>> GetPositionList(PagingRequestModel paging, string searchTerm, int? filterStatus);

        Task<PositionDetailResponse> GetPositionDetail(int id);
    }
}
