using BusinessLayer.Payload.RequestModel.CourseRequest;
using BusinessLayer.Payload.RequestModel.CriteriaRequest;
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
    }
}
