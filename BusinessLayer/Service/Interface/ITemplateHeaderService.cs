using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Payload.RequestModel.CriteriaRequest;
using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;
using BusinessLayer.Payload.ResponseModel.TemplateResponse;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITemplateHeaderService
    {
        Task<List<ListTemplateHeaderCriteriaResponse>> GetCriteriaTemplateHeader(int templateId);

        Task AddTemplateHeader(int templateId, CreateTemplateHeaderRequest request);

        Task UpdateTemplateHeader(int templateId, UpdateTemplateHeaderRequest request);

        Task DisableTemplateHeader(int templateId);

        Task ActiveTemplateHeader(int templateId);
    }
}
