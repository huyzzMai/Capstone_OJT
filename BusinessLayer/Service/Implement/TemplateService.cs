using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.TemplateHeaderRequest;
using BusinessLayer.Payload.RequestModel.TemplateRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.TemplateResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TemplateService : ITemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TemplateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task CreateTemplate(CreateTemplateRequest request)
        {
            try
            {
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c => c.Id == request.UniversityId 
                && c.Status==CommonEnums.UNIVERSITY_STATUS.ACTIVE);             
                if(uni == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "University not found");
                }
                var Temp = new Template()
                {
                    Name = request.Name,
                    Url = request.Url,
                    StartCell = request.StartCell,
                    UniversityId = request.UniversityId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.TEMPLATE_STATUS.ACTIVE
                };
                await _unitOfWork.TemplateRepository.Add(Temp);
                foreach (var i in request.TemplateHeaders)
                {

                    var newtemp = new TemplateHeader()
                    {
                        TemplateId = Temp.Id,
                        Name = i.Name,
                        IsCriteria = i.IsCriteria,
                        MatchedAttribute = i.MatchedAttribute,
                        Order = i.Order,
                        FormulaId = i.FormulaId   ,
                        TotalPoint = i.TotalPoint,
                        Status = CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UpdatedAt = DateTime.UtcNow.AddHours(7),

                    };
                    await _unitOfWork.TemplateHeaderRepository.Add(newtemp);                  
                }

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

        public async Task DisableTemplate(int templateId)
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

                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Template is used in active batch");
                }
                temp.Status = CommonEnums.TEMPLATE_STATUS.INACTIVE;
                await _unitOfWork.TemplateRepository.Update(temp);
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

        public async Task ActiveTemplate(int templateId)
        {
            try
            {
                var temp = await _unitOfWork.TemplateRepository.GetFirst(c => c.Id == templateId);
                if (temp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Template not found");
                }
                temp.Status = CommonEnums.TEMPLATE_STATUS.ACTIVE;
                await _unitOfWork.TemplateRepository.Update(temp);
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

        public async Task<TemplateDetailResponse> GetTemplateDetail(int templateId)
        {
            try
            {
                var temp = await _unitOfWork.TemplateRepository.GetFirst(c => c.Id == templateId, "TemplateHeaders","University");
                if (temp == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template not found");
                }
                var tempresponse = new TemplateDetailResponse()
                {
                    Id = templateId,
                    Name = temp.Name,
                    StartCell = temp.StartCell,
                    Status = temp.Status,
                    UniversityId = temp.UniversityId,
                    UniversityName=temp.University.Name,
                    UpdatedAt = DateTimeService.ConvertToDateString(temp.UpdatedAt),
                    CreatedAt = DateTimeService.ConvertToDateString(temp.CreatedAt),
                    Url = temp.Url,
                    templateHeaders = temp.TemplateHeaders.OrderBy(c=>c.Order).Select(cp =>
                        new TemplateHeaderResponse()
                        {
                            Id = cp.Id,
                            Name = cp.Name,
                            MatchedAttribute = cp.MatchedAttribute,
                            TotalPoint = cp.TotalPoint,
                            FormulaId= cp.FormulaId,
                            IsCriteria = cp.IsCriteria,
                            Order = cp.Order,
                            Status = cp.Status
                        }).ToList()
                };
                return tempresponse;
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
        public List<Template> SearchTemplate(string searchTerm, int? filterstatus, List<Template> courselist)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
            }

            var query = courselist.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm)
                );
            }
            if (filterstatus != null)
            {
                query = query.Where(c => c.Status == filterstatus);
            }          
            return query.ToList();
        }
        public async Task<BasePagingViewModel<ListTemplateResponse>> GetTemplateList(PagingRequestModel paging, string searchTerm, int? filterstatus)
        {
            try
            {
                var list = await _unitOfWork.TemplateRepository.Get(includeProperties:"University");
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template list not found");
                }
                 if (!string.IsNullOrEmpty(searchTerm) ||filterstatus != null)
                {
                    list = SearchTemplate(searchTerm, filterstatus, list.ToList());
                }
                var listresponse = list.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new ListTemplateResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StartCell = c.StartCell,
                        Status = c.Status,
                        UniversityId = c.UniversityId,
                        UniversityName=c.University.Name,
                        Url = c.Url
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                  .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<ListTemplateResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = listresponse
                };
                return result;
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

        public async Task UpdateTemplate(int templateId, UpdateTemplateRequest request)
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
                   
                     throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Template is used in active batch");
                }
                temp.Name = request.Name;
                temp.Status = request.Status;
                temp.StartCell = request.StartCell;
                temp.Url = request.Url;
                temp.UniversityId = request.UniversityId;
                temp.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.TemplateRepository.Update(temp);
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

        

        public async Task<List<ListTemplateUniversityResponse>> GetTemplateUniversityList(int uniId)
        {
            try
            {
                var list = await _unitOfWork.TemplateRepository.Get(c => c.Status == CommonEnums.UNIVERSITY_STATUS.ACTIVE && c.University.Id==uniId ,"University");
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template list not found");
                }                
                var listresponse = list.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new ListTemplateUniversityResponse()
                    {
                        Id = c.Id,
                        Name = c.Name                     
                    };
                }
                ).ToList();
               
                return listresponse;
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
