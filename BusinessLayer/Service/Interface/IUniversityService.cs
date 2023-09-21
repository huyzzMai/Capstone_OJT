using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Payload.ResponseModel.UniversityResponse;
using BusinessLayer.Payload.RequestModel.UniversityRequest;

namespace BusinessLayer.Service.Interface
{
    public interface IUniversityService
    {
        Task<BasePagingViewModel<UniversityListResponse>> GetUniversityList(PagingRequestModel paging, string searchTerm,int? filterStatus);

        Task<UniversityDetailResponse> GetDetailUniversityId(int Id);

        Task UpdateUniversity (int universityId,UpdateUniversityRequest request);

        Task DisableUniversity(int universityId);

        Task ActiveUniversity(int universityId);

        Task CreateUniversuty(CreateUniversityRequest request);
    }
}
