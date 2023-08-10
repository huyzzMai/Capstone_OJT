using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Models.ResponseModel.CriteriaResponse;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrelloDotNet;

namespace BusinessLayer.Service.Implement
{
    public class UserCriteriaService : IUserCriteriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IConfiguration _configuration;
        public UserCriteriaService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public double? GetPointByFomular(int userId, int templateHeaderId)
        {
            var templateheader = _unitOfWork.TemplateHeaderRepository.GetFirst(c => c.Id == templateHeaderId, "Formula").Result;
            if (templateheader.FormulaId == null)
            {
                return null;
            }
            var counting = CountTaskOfTrainee(userId).Result;
            string formular = templateheader.Formula.Calculation;
            OperandsHandler handler = new OperandsHandler(userId, _unitOfWork, _configuration,counting);
            var methodNames = Regex.Matches(formular, @"\(([^)]*)\)").Cast<Match>().Select(match => match.Groups[1].Value).ToList();
            foreach (var methodName in methodNames)
            {
                var method = typeof(OperandsHandler).GetMethod(methodName);
                if (method != null)
                {
                    var result = (int)method.Invoke(handler, null);
                    formular = formular.Replace($"({methodName})", result.ToString());
                }
            }
            var engine = new NCalc.Expression(formular);
            var finalResult = engine.Evaluate();
            return (double?)finalResult;
        }
        public async Task<List<UserCriteriaResponse>> GetUserCriteria(int tranerId, int ojtBatchId)
        {
            try
            {
                var users = await _unitOfWork.UserRepository.Get(c => c.UserReferenceId == tranerId && c.OJTBatchId == ojtBatchId);
                if(users == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainee not found");
                }
                var res = users.Select(
                user =>
                {
                    return new UserCriteriaResponse()
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RollNumber = user.RollNumber,
                        Criterias = user.UserCriterias.OrderBy(c => c.TemplateHeader.Order).Select(c =>
                        new CriteriaResponse()
                        {
                            Id = c.TemplateHeaderId,
                            TotalPoint = c.TemplateHeader.TotalPoint,
                            Point = GetPointByFomular(user.Id, c.TemplateHeaderId)
                        }
                        ).ToList()
                    };
                }
                ).ToList();
                return res;
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
        public async Task<TaskCounterResponse> CountTaskOfTrainee(int traineeId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }
                var trelloUserId = user.TrelloId;
                var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var cards = await client.GetCardsForMemberAsync(trelloUserId);
                int totalTask = cards.Count();

                var doneTasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedDoneOfTrainee(traineeId);
                int totalDoneTask = doneTasks.Count();

                int totalOverdueTask = totalTask - totalDoneTask;

                TaskCounterResponse result = new()
                {
                    TotalTask = totalTask,
                    TaskComplete = totalDoneTask,
                    TaskOverdue = totalOverdueTask
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdatePoints(List<UpdateCriteriaRequest> requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==request.UserId && c.Status==CommonEnums.USER_STATUS.ACTIVE);
                    if (user==null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"Trainee not found");
                    }    
                    foreach(var item in request.UserCriterias)
                    {
                        var usercriteria = await _unitOfWork.UserCriteriaRepository.GetFirst(c=>c.TemplateHeaderId==item.TemplateHeaderId && c.UserId==user.Id);
                        usercriteria.Point = item.Point;
                        await _unitOfWork.UserCriteriaRepository.Update(usercriteria);
                    }
                }
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
