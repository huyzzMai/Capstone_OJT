using BusinessLayer.Payload.RequestModel.CriteriaRequest;
using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;
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
        public async Task AddTemplateHeader(int templateId, CreateTemplateHeaderRequest request)
        {
            try
            {
                var temp = await _unitOfWork.TemplateRepository.GetFirst(c => c.Id == templateId);
                if (temp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Template not found");
                }
                var activeojt = await _unitOfWork.OJTBatchRepository.GetlistActiveOjtbatchWithTemplate(templateId);
                if (activeojt.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Template header is used in active batch");
                }
                var temheader = new TemplateHeader()
                {
                    Name = request.Name,
                    IsCriteria = request.IsCriteria,
                    MatchedAttribute = request.MatchedAttribute,
                    Order = request.Order,
                    Status = CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE,
                    FormulaId = request.FormulaId,
                    TemplateId = templateId,
                    TotalPoint = request.TotalPoint,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7)
                };
                await _unitOfWork.TemplateHeaderRepository.Add(temheader);
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

        public async Task UpdateTemplateHeader(int templateId, UpdateTemplateHeaderRequest request)
        {
            try
            {
                var temp = await _unitOfWork.TemplateHeaderRepository.GetFirst(c => c.Id == templateId, "UserCriterias");
                if (temp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Template not found");
                }
                var activeojt = await _unitOfWork.OJTBatchRepository.GetlistActiveOjtbatchWithTemplate(templateId);
                if (activeojt.Any())
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Template header is used in active batch");
                }
                temp.MatchedAttribute = request.MatchedAttribute;
                temp.Name = request.Name;
                temp.Order = request.Order;
                temp.Status = request.Status;
                temp.IsCriteria= request.IsCriteria;
                temp.FormulaId = request.FormulaId;
                temp.TotalPoint = request.TotalPoint;
                temp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TemplateHeaderRepository.Update(temp);
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

        public async Task DisableTemplateHeader(int templateId)
        {
            try
            {
                var tempheader = await _unitOfWork.TemplateHeaderRepository.GetFirst(c => c.Id == templateId);
                if (tempheader == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Template header not found");
                }
                var activeojt = await _unitOfWork.OJTBatchRepository.GetlistActiveOjtbatchWithTemplate(tempheader.TemplateId);
                if (activeojt.Any())
                {

                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Template header is used in active batch");
                }
                tempheader.Status = CommonEnums.TEMPLATEHEADER_STATUS.INACTIVE;
                await _unitOfWork.TemplateHeaderRepository.Update(tempheader);
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

        public async Task ActiveTemplateHeader(int templateId)
        {
            try
            {
                var tempheader = await _unitOfWork.TemplateHeaderRepository.GetFirst(c => c.Id == templateId);
                if (tempheader == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Template header not found");
                }
                tempheader.Status = CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE;
                await _unitOfWork.TemplateHeaderRepository.Update(tempheader);
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
