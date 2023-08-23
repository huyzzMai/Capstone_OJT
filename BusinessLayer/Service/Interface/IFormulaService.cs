using BusinessLayer.Models.RequestModel.SkillRequest;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.ResponseModel.SkillResponse;
using BusinessLayer.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models.RequestModel.FormulaRequest;
using BusinessLayer.Models.ResponseModel.FormulaResponse;

namespace BusinessLayer.Service.Interface
{
    public interface IFormulaService
    {
        Task CreateFormula(CreateFormulaRequest request);

        Task UpdateFormula(int formulaId, UpdateFormulaRequest request);

        Task DeleteFormula(int formulaId);

        Task<BasePagingViewModel<FormulaResponse>> GetFormulaList(PagingRequestModel paging, string searchTerm, int? filterStatus);

        Task<FormularDetailResponse> GetFormulaDetail(int formulaId);
    }
}
