using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models.ResponseModel.UniversityResponse;
using BusinessLayer.Models.RequestModel.UniversityRequest;

namespace BusinessLayer.Service.Interface
{
    public interface IUniversityService
    {
        Task<BasePagingViewModel<UniversityListResponse>> GetUniversityList(PagingRequestModel paging, string searchTerm,int? filterStatus);

        Task<UniversityDetailResponse> GetDetailUniversityId(int Id);

        Task UpdateUniversity (int universityId,UpdateUniversityRequest request);

        Task DeleteUniversity(int universityId);

        Task CreateUniversuty(CreateUniversityRequest request);
    }
}
