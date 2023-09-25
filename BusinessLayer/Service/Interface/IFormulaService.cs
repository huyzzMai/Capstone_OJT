using BusinessLayer.Payload.RequestModel.SkillRequest;
using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.ResponseModel.SkillResponse;
using BusinessLayer.Payload.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Payload.RequestModel.FormulaRequest;
using BusinessLayer.Payload.ResponseModel.FormulaResponse;

namespace BusinessLayer.Service.Interface
{
    public interface IFormulaService
    {
        Task CreateFormula(CreateFormulaRequest request);

        Task UpdateFormula(int formulaId, UpdateFormulaRequest request);

        Task DisableFormula(int formulaId);

        Task ActiveFormula(int formulaId);

        Task<BasePagingViewModel<FormulaResponse>> GetFormulaList(PagingRequestModel paging, string searchTerm, int? filterStatus);

        Task<FormularDetailResponse> GetFormulaDetail(int formulaId);
    }
}
