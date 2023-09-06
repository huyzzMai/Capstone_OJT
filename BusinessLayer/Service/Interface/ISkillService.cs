using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel;
using System.Threading.Tasks;
using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.ResponseModel.SkillResponse;

namespace BusinessLayer.Service.Interface
{
    public interface ISkillService
    {
        Task CreateSkill(CreateSkillRequest request);

        Task UpdateSkill(int skillId, UpdateSkillRequest request);

        Task DeleteSkill(int skillId);

        Task<BasePagingViewModel<SkillResponse>> GetSkillList(PagingRequestModel paging,string searchTerm,int? filterStatus);

        Task<SkillDetailResponse> GetSkillDetail(int skillId);
    }
}
