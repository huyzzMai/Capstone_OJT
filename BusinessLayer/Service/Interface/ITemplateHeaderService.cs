using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Models.ResponseModel.TemplateResponse;
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
        Task<List<ListTemplateHeaderResponse>> GetCriteriaTemplateHeader(int templateId);
    }
}
