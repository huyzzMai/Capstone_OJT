using BusinessLayer.Payload.RequestModel.CriteriaRequest;
using BusinessLayer.Payload.ResponseModel.TemplateResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TemplateHeaderService : ITemplateHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TemplateHeaderService(IUnitOfWork unitOfWork) 
         {
             _unitOfWork = unitOfWork;
         }

        public async Task<List<ListTemplateHeaderCriteriaResponse>> GetCriteriaTemplateHeader(int templateId)
        {
            try
            {
                
                var list = await _unitOfWork.TemplateHeaderRepository.Get(c=>c.TemplateId==templateId && c.IsCriteria==true);
                if(list==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"There is no criteria template header");
                }
                var response= list.OrderBy(c=>c.Order).Select(
                    r=>
                    {
                        return new ListTemplateHeaderCriteriaResponse
                        {
                            TeamplateHeaderId = r.Id,
                            Name = r.Name,
                            MaxPoint=r.TotalPoint
                        };
                    }                  
                    ).ToList();
                return response;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
