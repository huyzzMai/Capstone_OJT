using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.UniversityRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.OJTBatchResponse;
using BusinessLayer.Payload.ResponseModel.UniversityResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class UniversityService : IUniversityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UniversityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UniversityDetailResponse> GetDetailUniversityId(int Id)
        {
            try
            {
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c => c.Id==Id, "OJTBatches");
                if (uni == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "University not found");
                }
                var uniresponse = new UniversityDetailResponse()
                {
                    Id = Id,
                    ImgURL = uni.ImgURL,
                    Name = uni.Name,
                    Address = uni.Address,
                    JoinDate = DateTimeService.ConvertToDateString(uni.JoinDate),
                    CreatedAt = DateTimeService.ConvertToDateString(uni.CreatedAt),
                    UpdatedAt = DateTimeService.ConvertToDateString(uni.UpdatedAt),
                    Status = uni.Status,
                    validOJTBatchResponses = uni.OJTBatches.Select(cp =>
                        new ValidOJTBatchResponse()
                        {
                            Id = cp.Id,
                            Name = cp.Name,
                            StartTime = DateTimeService.ConvertToDateString(cp.StartTime),
                            EndTime = DateTimeService.ConvertToDateString(cp.EndTime)
                        }).ToList()
                };
                return uniresponse;
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


        public List<University> SearchUniversities(string searchTerm,int? filterStatus,List<University> unilist)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
            }

            var query = unilist.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm)
                );
            }
            if(filterStatus != null)
            {
                query = query.Where(c =>c.Status==filterStatus
               );
            }
            return query.ToList();
        }
        public int CountTotalUsersInUniversity(List<University> uni)
        {
            var university = uni
                .Select(u => new
                {
                    UniversityId = u.Id,
                    TotalUsers = u.OJTBatches.SelectMany(b => b.Trainees)
                                           .Count(user => user.Status != CommonEnums.USER_STATUS.DELETED)
                })
                .FirstOrDefault();
            return university?.TotalUsers ?? 0;
        }
        public int CountUsersWithStatusInUniversity(List<University> uni, int status)
        {
            var university = uni
                .Select(u => new
                {
                    UniversityId = u.Id,
                    TotalUsers = u.OJTBatches.SelectMany(b => b.Trainees)
                                           .Count(user => user.Status == status)
                })
                .FirstOrDefault();

            return university?.TotalUsers ?? 0;
        }
        public async Task<BasePagingViewModel<UniversityListResponse>> GetUniversityList(PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                var list = await _unitOfWork.UniversityRepository.Get(includeProperties:"OJTBatches");
                var listuser = await _unitOfWork.UserRepository.Get();
                if (list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty list");
                }
                if(!string.IsNullOrEmpty(searchTerm) || filterStatus != null)
                {
                    list = SearchUniversities(searchTerm, filterStatus, list.ToList());
                }             
                var listresponse = list.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new UniversityListResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Address = c.Address,
                        ImgURL = c.ImgURL,
                        JoinDate = DateTimeService.ConvertToDateString(c.JoinDate),
                        Status = c.Status,
                        TotalBatches = c.OJTBatches.Where(c => c.IsDeleted != false).ToList().Count,
                        OjtTrainees = CountTotalUsersInUniversity(list.Where(cd => cd.Id == c.Id).ToList()),
                        OjtActiveTrainees = CountUsersWithStatusInUniversity(list.Where(cd => cd.Id == c.Id).ToList(), CommonEnums.USER_STATUS.ACTIVE)
                    };
                }
                ).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<UniversityListResponse>()
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

        public async Task UpdateUniversity(int universityId, UpdateUniversityRequest request)
        {
            try
            {
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c => c.Id == universityId);
                if (uni == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "University not found");
                }
                if (uni.Name.ToLower() != request.Name.ToLower())
                {
                    var ok = await _unitOfWork.UniversityRepository.Get();
                    var check = ok.Any(c => c.Name.ToLower() == request.Name.ToLower());
                    if (check)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Name university already in use");
                    }
                }               
                uni.Name = request.Name;
                uni.Address = request.Address;
                uni.ImgURL = request.ImgURL;
                uni.JoinDate = request.JoinDate;
                uni.Status = request.Status;
                uni.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.UniversityRepository.Update(uni);
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

        public async Task DeleteUniversity(int universityId)
        {
            try
            {
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c => c.Id == universityId, "OJTBatches");
                if (uni == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "University not found");
                }
                var check = uni.OJTBatches.Any(c => c.IsDeleted == false);
                if (check)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "University still have some active batches");
                }
                uni.Status = CommonEnums.UNIVERSITY_STATUS.INACTIVE;
                await _unitOfWork.UniversityRepository.Update(uni);
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

        public async Task CreateUniversuty(CreateUniversityRequest request)
        {
            try
            {
                var ok = await _unitOfWork.UniversityRepository.Get();
                var check = ok.Any(c => c.Name.ToLower() == request.Name.ToLower());
                if (check)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Name university already in use");
                }
                var updateuni = new University()
                {                  
                    Name = request.Name,
                    Address = request.Address,
                    ImgURL = request.ImgURL,
                    JoinDate = request.JoinDate,
                    Status = CommonEnums.UNIVERSITY_STATUS.ACTIVE,
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                    CreatedAt = DateTime.UtcNow.AddHours(7)                   
                };
                await _unitOfWork.UniversityRepository.Add(updateuni);
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
