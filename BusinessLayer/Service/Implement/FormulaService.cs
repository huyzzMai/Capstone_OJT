using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.FormulaRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.FormulaResponse;
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
using NCalc;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;

namespace BusinessLayer.Service.Implement
{
    public class FormulaService: IFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FormulaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        static bool IsExpressionValid(string expression)
        {
            try
            {
                Random random = new Random();
                string pattern = @"\b[a-zA-Z]+\b";

                List<string> variables = new List<string>();
               if (expression.Contains("/0"))
                {
                    return false;
                }
                MatchEvaluator evaluator = (match) =>
                {
                    variables.Add(match.Value); 
                    return "0"; 
                };
                string replacedExpression = Regex.Replace(expression, pattern, evaluator);
                Expression ncalcExpression = new  Expression(expression); ;             
                foreach (var variable in variables)
                {
                    int randomValue = random.Next(1, 100);
                    ncalcExpression.Parameters[variable] = randomValue.ToString();
                }

                object result = ncalcExpression.Evaluate();
                if (result is double && double.IsInfinity((double)result))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task CreateFormula(CreateFormulaRequest request)
        {
           try
            {
                var formula = await _unitOfWork.FormulaRepository.
                    GetFirst(c => c.Name.ToLower() == request.Name.Trim().ToLower() 
                    && c.Status == CommonEnums.FORMULA_STATUS.ACTIVE);
                if (formula != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Name formula already exists");
                }
                if (!IsExpressionValid(request.Calculation))
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT,
                        "The expression is not valid.");
                };               
                
                var newformula = new Formula()
                {
                    Name= request.Name,
                    Calculation=request.Calculation,
                    Status=CommonEnums.FORMULA_STATUS.ACTIVE
                };
                await _unitOfWork.FormulaRepository.Add(newformula);
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

        public async Task DeleteFormula(int formulaId)
        {
            try
            {
                var formula = await _unitOfWork.FormulaRepository.
                    GetFirst(c=>c.Status==CommonEnums.FORMULA_STATUS.ACTIVE
                    && c.Id==formulaId
                    );
                if(formula == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"formula not found");
                }
                formula.Status = CommonEnums.FORMULA_STATUS.INACTIVE;
                await _unitOfWork.FormulaRepository.Update(formula);
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

        public async Task<FormularDetailResponse> GetFormulaDetail(int formulaId)
        {
            try
            {
                var formula = await _unitOfWork.FormulaRepository.
                   GetFirst(c => c.Status == CommonEnums.FORMULA_STATUS.ACTIVE
                   && c.Id == formulaId
                   );
                if (formula == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "formula not found");
                }
                var formuladetail = new FormularDetailResponse()
                {
                    Id = formulaId,
                    Name = formula.Name,
                    Calculation = formula.Calculation,
                    Status = formula.Status
                };
                return formuladetail;
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
        public List<Formula> SearchFormulas(string searchTerm, int? filterStatus, List<Formula> formulalist)
        {
            var query = formulalist.AsQueryable();
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
        public async Task<BasePagingViewModel<FormulaResponse>> GetFormulaList(PagingRequestModel paging, string searchTerm, int? filterStatus)
        {
            try
            {
                var list = await _unitOfWork.FormulaRepository.Get();
                if(list == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Empty list");
                }
                if (!string.IsNullOrEmpty(searchTerm) || filterStatus != null)
                {
                    list = SearchFormulas(searchTerm, filterStatus, list.ToList());
                }
                var listresponse= list.Select(c =>
                {
                    return new FormulaResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Status = c.Status
                    };
                }).ToList();
                int totalItem = listresponse.Count;
                listresponse = listresponse.Skip((paging.PageIndex - 1) * paging.PageSize)
                   .Take(paging.PageSize).ToList();
                var result = new BasePagingViewModel<FormulaResponse>()
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

        public async Task UpdateFormula(int formulaId, UpdateFormulaRequest request)
        {
            try
            {
                var formula = await _unitOfWork.FormulaRepository.
                   GetFirst(c => c.Status == CommonEnums.FORMULA_STATUS.ACTIVE
                   && c.Id == formulaId
                   );
                if (formula == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "formula not found");

                }
                var formulacheck = await _unitOfWork.FormulaRepository.GetFirst(c =>request.Name!=formula.Name && c.Name.ToLower() == request.Name.Trim().ToLower() 
                );

                if (formulacheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.CONFLICT, "Duplicate formula names");
                }
                formula.Calculation = request.Calculation;
                formula.Name = request.Name;
                formula.Status = request.Status;
                await _unitOfWork.FormulaRepository.Update(formula);
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
