using BusinessLayer.Models.RequestModel.OjtBatchRequest;
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
                 StartTime= batch.StartTime,
                 EndTime= batch.EndTime,
                 UniversityId= batch.UniversityId,
                 CreatedAt= batch.CreatedAt,
                 UpdatedAt= batch.UpdatedAt,
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

        public async Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtList()
        {
            try
            {
                var list = await _unitOfWork.OJTBatchRepository.Get(c => c.IsDeleted == false && c.EndTime > DateTime.UtcNow.AddHours(7));
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                IEnumerable<ValidOJTBatchResponse> res = list.Select(
                    ojt =>
                    {
                        return new ValidOJTBatchResponse()
                        {
                            Id = ojt.Id,
                            Name = ojt.Name,
                            StartTime = ojt.StartTime,
                            EndTime = ojt.EndTime
                        };
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
        public async Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtListbyUniversityId(int id)
        {
            try
            {
                var list = await _unitOfWork.OJTBatchRepository.Get(c => c.IsDeleted == false && c.UniversityId == id);
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty List OJTBatch");
                }
                IEnumerable<ValidOJTBatchResponse> res = list.Select(
                    ojt =>
                    {
                        return new ValidOJTBatchResponse()
                        {
                            Id = ojt.Id,
                            Name = ojt.Name,
                            StartTime = ojt.StartTime,
                            EndTime = ojt.EndTime
                        };
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
    }
}
