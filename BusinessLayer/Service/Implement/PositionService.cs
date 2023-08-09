using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.PositionRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.PositionResponse;
using BusinessLayer.Models.ResponseModel.SkillResponse;
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
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePosition(CreatePositionRequest request)
        {
            try
            {
                var position = await _unitOfWork.PositionRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() && c.Status == CommonEnums.POSITION_STATUS.ACTIVE);
                if (position != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Position already exists");
                }
                var positioncheck = await _unitOfWork.PositionRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() && c.Status == CommonEnums.POSITION_STATUS.ACTIVE);

                if (positioncheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate position names");
                }
                var newPosition = new Position()
                {
                    Name=request.Name,
                    Status=CommonEnums.POSITION_STATUS.ACTIVE,
                    CreatedAt=DateTimeService.GetCurrentDateTime(),
                    UpdatedAt=DateTimeService.GetCurrentDateTime()
                };
                await _unitOfWork.PositionRepository.Add(newPosition);
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

        public async Task DeletePosition(int id)
        {
            try
            {
                var position = await _unitOfWork.PositionRepository.GetFirst(c => c.Id == id, "CoursePositions", "Users");
                if (position == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Position not found");
                }                
                position.Status = CommonEnums.POSITION_STATUS.ACTIVE;
                await _unitOfWork.PositionRepository.Update(position);
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

        public async Task<PositionDetailResponse> GetPositionDetail(int id)
        {
            try
            {
                var position = await _unitOfWork.PositionRepository.GetFirst(c => c.Id == id);
                if (position == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Position not found");
                }
                var positiondetail = new PositionDetailResponse()
                {
                    Id = position.Id,
                    Name = position.Name,
                    Status = position.Status,
                    CreatedAt=DateTimeService.ConvertToDateString(position.CreatedAt),
                    UpdatedAt=DateTimeService.ConvertToDateString(position.UpdatedAt)
                };
                return positiondetail;

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
        public List<Position> SearchPositions(string searchTerm, int? filterStatus, List<Position> positions)
        {
            var query = positions.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm)
            );
            }
            if (filterStatus != null)
            {
                query = query.Where(c => c.Status == filterStatus);
            }
            return query.ToList();
        }
        public async Task<BasePagingViewModel<ListPostionResponse>> GetPositionList(PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                var listposition = await _unitOfWork.PositionRepository.Get();
                if (!string.IsNullOrEmpty(searchTerm) || filterStatus != null)
                {
                    listposition = SearchPositions(searchTerm, filterStatus, listposition.ToList());
                }
                var listresponse = listposition.Select(c =>
                {
                    return new ListPostionResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Status=c.Status                        
                    };
                }).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<ListPostionResponse>()
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

        public async Task UpdatePositon(int id, UpdatePositionRequest request)
        {
            try
            {
                var position = await _unitOfWork.PositionRepository.GetFirst(c => c.Id == id && c.Status == CommonEnums.POSITION_STATUS.ACTIVE);
                if (position == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Position not found");
                }
                var positioncheck = await _unitOfWork.SkillRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() && c.Status == CommonEnums.POSITION_STATUS.ACTIVE);

                if (positioncheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate position names");
                }
                position.Name = request.Name;
                position.Status = request.Status;
                position.UpdatedAt = DateTimeService.GetCurrentDateTime();
                await _unitOfWork.PositionRepository.Update(position);
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
