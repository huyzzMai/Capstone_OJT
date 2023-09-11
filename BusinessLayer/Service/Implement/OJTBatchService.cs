using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.OjtBatchRequest;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.OJTBatchResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
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
    public class OJTBatchService : IOJTBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OJTBatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOjtBatch(CreateOjtBatchRequest request)
        {
            try
            {
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c=>c.Id==request.UniversityId);
                if (uni==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"University not found");
                }
                var newbatch = new OJTBatch()
                {
                    Name = request.Name,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    TemplateId = request.TemplateId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    UniversityId = request.UniversityId,
                    IsDeleted=false
                };
                await _unitOfWork.OJTBatchRepository.Add(newbatch);
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

        public async Task DeleteOjtBatch(int id)
        {
           try
            {
                var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.IsDeleted == false && c.Id == id);
                if (batch==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"Ojt batch not found");
                }
                batch.IsDeleted = true;
                await _unitOfWork.OJTBatchRepository.Update(batch);
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

        public async Task<OjtBatchDetailResponse> GetDetailOjtBatch(int batchId)
        {
            try
            {
                var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c=>c.Id==batchId,"University");
                if(batch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "OJTBatch not found");
                }
                var batchresponse = new OjtBatchDetailResponse()
                {
                 Id = batch.Id,
                 Name = batch.Name,
                 StartTime= DateTimeService.ConvertToDateString(batch.StartTime),
                 EndTime= DateTimeService.ConvertToDateString(batch.EndTime),
                 UniversityId= batch.UniversityId,
                 UniversityName=batch.University.Name,
                 TemplateId= batch.TemplateId,
                 CreatedAt= DateTimeService.ConvertToDateString(batch.CreatedAt),
                 UpdatedAt= DateTimeService.ConvertToDateString(batch.UpdatedAt),
                 IsDeleted= batch.IsDeleted
                };
                return batchresponse;
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

        public async Task<List<ListActiveOjtbatchforTrainer>> getListGradePointOjtbatch(int trainerId)
        {
           try
            {
                var listOjt = await _unitOfWork.OJTBatchRepository.Get(c=>c.Trainees.Any(c=>c.UserReferenceId == trainerId) 
                && c.IsDeleted==false 
                && c.EndTime.Value.AddDays(7) >= DateTimeService.GetCurrentDateTime(),"Trainees","University");
                if (listOjt == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                var res = listOjt.Select(
                    ojt =>
                    {
                        if (ojt.Trainees.Any(c => c.UserCriterias.Any(c => c.Point == null) || c.UserCriterias.Count < 1))
                        {
                            return new ListActiveOjtbatchforTrainer()
                            {
                                Id = ojt.Id,
                                Name = ojt.Name,
                                StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                                EndTime = DateTimeService.ConvertToDateString(ojt.EndTime),
                                NumberTrainee = ojt.Trainees.Count(),
                                UniversityCode = ojt.University.UniversityCode,
                                UniversityName=ojt.University.Name,
                                Status="Not Grade yet"
                            };
                        }
                        else
                        {
                            return new ListActiveOjtbatchforTrainer()
                            {
                                Id = ojt.Id,
                                Name = ojt.Name,
                                StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                                EndTime = DateTimeService.ConvertToDateString(ojt.EndTime),
                                NumberTrainee = ojt.Trainees.Count(),
                                UniversityCode = ojt.University.UniversityCode,
                                UniversityName = ojt.University.Name,
                                Status = "Graded"

                            };
                        }
                    }
                    ).ToList();
                return res;
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
        public List<ListOjtExport> SearchOjt(string Status,string searchTerm,List<ListOjtExport> list)
        {
            if (!string.IsNullOrEmpty(Status))
            {
                Status = Status.ToLower();
            }

            var query = list.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm=searchTerm.ToLower();
                query = query.Where(c=>c.UniversityName.ToLower().Contains(searchTerm) ||c.Name.ToLower().Contains(searchTerm));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(c =>
                    c.Status.ToLower().Contains(Status)
                );
            }          
            return query.ToList();
        }
        public async Task<BasePagingViewModel<ListOjtExport>> getListOjtbatchExportStatus(PagingRequestModel paging,string searchTerm, string Status)
        {
            try
            {
                var list = await _unitOfWork.OJTBatchRepository.Get(o=>o.IsDeleted==false,"Trainees","University", "Template");
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                var res = list.Select(
                    ojt =>
                    {
                        if (ojt.Trainees.Any(c => c.UserCriterias.Any(c => c.Point == null) || c.UserCriterias.Count < 1 ))
                        {
                            return new ListOjtExport()
                            {
                                Id = ojt.Id,
                                Name = ojt.Name,
                                StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                                EndTime = DateTimeService.ConvertToDateString(ojt.EndTime),
                                Status="Can not export",
                                Url=ojt.Template.Url,
                                UniversityName=ojt.University.Name
                            };
                        }
                        else
                        {
                            return new ListOjtExport()
                            {
                                Id = ojt.Id,
                                Name = ojt.Name,
                                StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                                EndTime = DateTimeService.ConvertToDateString(ojt.EndTime),
                                Status = "Can export",
                                Url = ojt.Template.Url,
                                UniversityName = ojt.University.Name
                            };
                        }
                    }
                    ).ToList();
                if (!string.IsNullOrEmpty(Status)|| !string.IsNullOrEmpty(searchTerm))
                {
                    res = SearchOjt(Status,searchTerm ,res);
                }
                int totalItem =res.Count;
                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                 .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<ListOjtExport>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
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

        public async Task<BasePagingViewModel<ValidOJTBatchResponse>> GetValidOJtList(PagingRequestModel paging)
        {
            try
            {
                var list = await _unitOfWork.OJTBatchRepository.Get(c => c.IsDeleted == false && c.EndTime > DateTime.UtcNow.AddHours(7));
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                var res = list.Select(
                    ojt =>
                    {
                        return new ValidOJTBatchResponse()
                        {
                            Id = ojt.Id,
                            Name = ojt.Name,
                            StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                            EndTime = DateTimeService.ConvertToDateString(ojt.EndTime)
                        };
                    }
                    ).ToList();
                int totalItem = res.Count;
                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<ValidOJTBatchResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
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
        public async Task<BasePagingViewModel<ValidOJTBatchResponse>> GetValidOJtListbyUniversityId(int id, PagingRequestModel paging)
        {
            try
            {
                var list = await _unitOfWork.OJTBatchRepository.Get(c => c.IsDeleted == false && c.UniversityId == id);
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                var res = list.Select(
                    ojt =>
                    {
                        return new ValidOJTBatchResponse()
                        {
                            Id = ojt.Id,
                            Name = ojt.Name,
                            TemplateId=ojt.TemplateId,
                            StartTime = DateTimeService.ConvertToDateString(ojt.StartTime),
                            EndTime = DateTimeService.ConvertToDateString(ojt.EndTime)
                        };
                    }
                    ).ToList();
                int totalItem = res.Count;
                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<ValidOJTBatchResponse>()
                {
                    PageIndex = paging.PageIndex,
                    PageSize = paging.PageSize,
                    TotalItem = totalItem,
                    TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                    Data = res
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

        public async Task UpdateOjtBatch(int id, UpdateOjtBatchRequest request)
        {
           try
            {
                var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.IsDeleted == false && c.Id == id);
                if (batch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Ojt batch not found");
                }
                batch.Name = request.Name;
                batch.UpdatedAt=DateTimeService.GetCurrentDateTime();
                batch.StartTime=request.StartTime; 
                batch.EndTime=request.EndTime;
                batch.UniversityId = request.UniversityId;
                batch.IsDeleted = request.IsDeleted;
                batch.TemplateId= request.TemplateId;
                await _unitOfWork.OJTBatchRepository.Update(batch);

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
