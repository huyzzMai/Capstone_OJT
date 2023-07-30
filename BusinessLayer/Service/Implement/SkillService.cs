using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.SkillRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.CourseResponse;
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
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateSkill(CreateSkillRequest request)
        {
            try
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() && c.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                if (skill != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT,"Skill already exists");
                }
                var newskill = new Skill()
                {
                    Name=request.Name,
                    Type=request.Type,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Status = CommonEnums.SKILL_STATUS.ACTIVE
                };
                await _unitOfWork.SkillRepository.Add(newskill);
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

        public async Task DeleteSkill(int skillId)
        {
            try
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId && c.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Skill not found");
                }
                skill.Status = CommonEnums.SKILL_STATUS.DELETED;
                await _unitOfWork.SkillRepository.Update(skill);
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

        public async Task<SkillDetailResponse> GetSkillDetail(int skillId)
        {
            try
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId && c.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Skill not found");
                }
                var skilldetail= new SkillDetailResponse()
                {
                    Id = skillId,
                    Name = skill.Name,
                    Type = skill.Type,
                    Status = skill.Status,
                    CreatedAt = skill.CreatedAt,
                    UpdatedAt = skill.UpdatedAt
                };
                return skilldetail;

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
        public List<Skill> SearchSkills(string? searchTerm, List<Skill> skilllist)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
            }

            var query = skilllist.AsQueryable();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm)
            );
            return query.ToList();
        }
        public async Task<BasePagingViewModel<SkillResponse>> GetSkillList(PagingRequestModel paging, string searchTerm)
        {
            try
            {
                var listskill = await _unitOfWork.SkillRepository.Get(c => c.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    listskill = SearchSkills(searchTerm, listskill.ToList());
                }
                var listresponse = listskill.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new SkillResponse()
                    {
                        Id= c.Id,
                        Name= c.Name,
                        Status =c.Status,
                        Type = c.Type
                    };
                }).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<SkillResponse>()
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
        public async Task UpdateSkill(int skillId, UpdateSkillRequest request)
        {
            try
            {         
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId && c.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Skill not found");
                }
                var skillcheck = await _unitOfWork.SkillRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower()&&c.Status==CommonEnums.SKILL_STATUS.ACTIVE);

                if (skillcheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate skill names");
                }
                skill.Name = request.Name;
                skill.Type= request.Type;
                skill.Status = request.Status;
                skill.UpdatedAt= DateTime.Now;
                await _unitOfWork.SkillRepository.Update(skill);
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
