using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.RequestModel.TemplateRequest;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models.ResponseModel.TemplateResponse;
using BusinessLayer.Models.RequestModel.TemplateHeaderRequest;

namespace BusinessLayer.Service.Interface
{
    public interface ITemplateService
    {
        Task CreateTemplate(CreateTemplateRequest request);

        Task UpdateTemplate(int templateId, UpdateTemplateRequest request);

        Task DeleteTemplate(int templateId);

        Task<BasePagingViewModel<ListTemplateResponse>> GetTemplateList(PagingRequestModel paging,string searchTerm, int? filterstatus);

        Task<TemplateDetailResponse> GetTemplateDetail(int templateId);

        Task AddTemplateHeader(int templateId, CreateTemplateHeaderRequest request);

        Task UpdateTemplateHeader(int templateId, UpdateTemplateHeaderRequest request);
    }
}
