using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.OjtBatchRequest;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.OJTBatchResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
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
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c=>c.Id==request.UniversityId && c.IsDeleted==false);
                if (uni==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"University not found");
                }
                var newbatch = new OJTBatch()
                {
                    Name = request.Name,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
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
                var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c=>c.Id==batchId);
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
