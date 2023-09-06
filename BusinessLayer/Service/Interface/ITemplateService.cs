using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Payload.RequestModel.TemplateRequest;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Payload.ResponseModel.TemplateResponse;
using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;

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
