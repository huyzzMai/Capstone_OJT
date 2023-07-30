using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CourseResponse;
using BusinessLayer.Models.ResponseModel.OJTBatchResponse;
using BusinessLayer.Models.ResponseModel.UniversityResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class UniversityService: IUniversityService
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
                var uni = await _unitOfWork.UniversityRepository.GetFirst(c=>c.IsDeleted==false,"OJTBatches");
                if(uni==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "University not found");
                }
                var uniresponse = new UniversityDetailResponse()
                {
                    Id = Id,
                    ImgURL = uni.ImgURL,
                    Name = uni.Name,
                    JoinDate = uni.JoinDate,
                    CreatedAt = uni.CreatedAt,
                    UpdatedAt = uni.UpdatedAt,
                    IsDeleted = uni.IsDeleted,
                    Status = uni.Status,
                    validOJTBatchResponses = uni.OJTBatches.Select(cp =>
                        new ValidOJTBatchResponse()
                        {
                            Id = cp.Id,
                            Name = cp.Name,
                            StartTime = cp.StartTime,
                            EndTime=cp.EndTime
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


        public List<University> SearchUniversities(string searchTerm, List<University> unilist)
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
            return query.ToList();
        }
        public async Task<BasePagingViewModel<UniversityListResponse>> GetUniversityList(PagingRequestModel paging, string searchTerm)
        {
            try
            {
                var list = await _unitOfWork.UniversityRepository.Get(c=>c.IsDeleted==false);
                if(list==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"Empty list");
                }
                list= SearchUniversities(searchTerm,list.ToList());
                var listresponse= list.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new UniversityListResponse()
                    {
                        Id = c.Id,
                        Name= c.Name,
                        ImgURL= c.ImgURL,
                        JoinDate= c.JoinDate,
                        Status=c.Status                      
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
    }
}
