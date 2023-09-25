using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.SkillResponse;
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
                var skillcheck = await _unitOfWork.SkillRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() && c.Status == CommonEnums.SKILL_STATUS.ACTIVE);

                if (skillcheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate skill names");
                }
                var newskill = new Skill()
                {
                    Name=request.Name,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
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

        public async Task DisableSkill(int skillId)
        {
            try
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId, "CourseSkills", "UserSkills");
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Skill not found");
                }
                var ck = skill.CourseSkills.Any(c => c.Course.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if(ck)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Delete fail! There are some active course which use this skill");
                }
                var uk= skill.UserSkills.Any(c=>c.User.Status == CommonEnums.USER_STATUS.ACTIVE);
                if(uk)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Delete fail! There are some active user which use this skill");
                }
                skill.Status = CommonEnums.SKILL_STATUS.INACTIVE;
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

        public async Task ActiveSkill(int skillId)
        {
            try
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId, "CourseSkills", "UserSkills");
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Skill not found");
                }
                var ck = skill.CourseSkills.Any(c => c.Course.Status == CommonEnums.COURSE_STATUS.ACTIVE);
                if (ck)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Delete fail! There are some active course which use this skill");
                }
                var uk = skill.UserSkills.Any(c => c.User.Status == CommonEnums.USER_STATUS.ACTIVE);
                if (uk)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Delete fail! There are some active user which use this skill");
                }
                skill.Status = CommonEnums.SKILL_STATUS.ACTIVE;
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
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId);
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Skill not found");
                }
                var skilldetail= new SkillDetailResponse()
                {
                    Id = skillId,
                    Name = skill.Name,
                    Status = skill.Status,
                    CreatedAt = DateTimeService.ConvertToDateString(skill.CreatedAt),
                    UpdatedAt = DateTimeService.ConvertToDateString(skill.UpdatedAt)
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
        public List<Skill> SearchSkills(string searchTerm,int? filterStatus, List<Skill> skilllist)
        {
            var query = skilllist.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm)
            );
            }         
            if (filterStatus != null)
            {
                query = query.Where(c => c.Status== filterStatus);
            }          
            return query.ToList();
        }
        public async Task<BasePagingViewModel<SkillResponse>> GetSkillList(PagingRequestModel paging, string searchTerm,int? filterStatus)
        {
            try
            {
                var listskill = await _unitOfWork.SkillRepository.Get();
                if (!string.IsNullOrEmpty(searchTerm)|| filterStatus != null)
                {
                    listskill = SearchSkills(searchTerm, filterStatus, listskill.ToList());
                }
                var listresponse = listskill.OrderByDescending(c => c.CreatedAt).Select(c =>
                {
                    return new SkillResponse()
                    {
                        Id= c.Id,
                        Name= c.Name,
                        Status= c.Status
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
                var skill = await _unitOfWork.SkillRepository.GetFirst(c => c.Id == skillId);
                if (skill == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Skill not found");
                }
                var skillcheck = await _unitOfWork.SkillRepository.GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower());

                if (skillcheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate skill names");
                }
                skill.Name = request.Name;
                skill.Status = request.Status;
                skill.UpdatedAt= DateTime.UtcNow.AddHours(7);
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
