using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel;
using System.Threading.Tasks;
using BusinessLayer.Models.RequestModel.SkillRequest;
using BusinessLayer.Models.ResponseModel.SkillResponse;

namespace BusinessLayer.Service.Interface
{
    public interface ISkillService
    {
        Task CreateSkill(CreateSkillRequest request);

        Task UpdateSkill(int skillId, UpdateSkillRequest request);

        Task DeleteSkill(int skillId);

        Task<BasePagingViewModel<SkillResponse>> GetSkillList(PagingRequestModel paging);
    }
}
