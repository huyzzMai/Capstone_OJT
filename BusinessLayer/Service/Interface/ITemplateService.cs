using BusinessLayer.Models.RequestModel.CourseRequest;
using BusinessLayer.Models.RequestModel.TemplateRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ITemplateService
    {
        Task CreateTemplate(CreateTemplateRequest request);
    }
}
