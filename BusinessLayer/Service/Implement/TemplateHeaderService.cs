using BusinessLayer.Models.RequestModel.CriteriaRequest;
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

        public async Task<List<string>> GetCriteriaTemplateHeader(int templateId)
        {
            try
            {
                List<string> strings = new List<string>();  
                var list = await _unitOfWork.TemplateHeaderRepository.Get(c=>c.TemplateId==templateId && c.IsCriteria==true);
                if(list==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"There is no criteria template header");
                }
                foreach (var item in list.OrderBy(c=>c.Order))
                {
                    strings.Add(item.Name);
                }
                return strings;
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
