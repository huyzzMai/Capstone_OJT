using BusinessLayer.Payload.RequestModel.CriteriaRequest;
using BusinessLayer.Payload.ResponseModel.CriteriaResponse;
using BusinessLayer.Payload.ResponseModel.TaskResponse;
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

        private readonly ITaskService _taskService; 
        public UserCriteriaService(IUnitOfWork unitOfWork, IConfiguration configuration, ITaskService taskService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _taskService = taskService;
        }
        public double? GetPointByFomular(int templateHeaderId,OperandsHandler handler)
        {
            var templateheader = _unitOfWork.TemplateHeaderRepository.GetFirst(c => c.Id == templateHeaderId, "Formula").Result;
            if (templateheader.FormulaId == null)
            {
                return null;
            }
           
            string formular = templateheader.Formula.Calculation;           
            var methodNames = Regex.Matches(formular, @"\(([^)]*)\)").Cast<Match>().Select(match => match.Groups[1].Value).ToList();
            foreach (var methodName in methodNames)
            {
                var method = typeof(OperandsHandler).GetMethod(methodName);
                if (method != null)
                {
                    var test = method.Invoke(handler, null);
                    if (test is int inttValue)
                    {
                        formular = formular.Replace($"({methodName})", inttValue.ToString());
                    }
                    else if (test is double doubleeValue)
                    {
                        formular = formular.Replace($"({methodName})", doubleeValue.ToString());
                    }
                }
            }
            var engine = new NCalc.Expression(formular);
            double finalResult=0;
            object result = engine.Evaluate();

            if (result is int intValue)
            {
                finalResult = Convert.ToDouble(intValue);
            }
            else if (result is double doubleValue)
            {
                finalResult = doubleValue;
            }
            return finalResult;
        }

        public async Task<List<UserCriteriaResponse>> GetUserCriteriaToGrade(int tranerId, int ojtBatchId)
        {
            try
            {             
                var users = await _unitOfWork.UserRepository.Get(c => c.UserReferenceId == tranerId && c.OJTBatchId == ojtBatchId, "UserCriterias");
                OperandsHandler handler=null;
                if (users == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainee not found");
                }
                var res = users.Select(
                user =>
                {
                    if(user.TrelloId != null)
                    {
                        var counting = _taskService.CountTaskOfTrainee(user.Id).Result;
                        handler = new OperandsHandler(user.Id, _unitOfWork, _configuration, counting);
                    }
                    else
                    {
                        handler = new OperandsHandler(user.Id, _unitOfWork, _configuration, null);
                    }
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
                            Point = GetPointByFomular(c.TemplateHeaderId,handler)
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

        //public async Task<TaskCounterResponse> CountTaskOfTrainee(int traineeId)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
        //        if (user == null)
        //        {
        //            throw new Exception("User not found!");
        //        }
        //        var trelloUserId = user.TrelloId;
        //        if(trelloUserId==null)
        //        {
        //            return null;
        //        }
        //        var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

        //        var cards = await client.GetCardsForMemberAsync(trelloUserId);

        //        int totalTask = cards.Count();

        //        var doneTasks = await _unitOfWork.TaskRepository.GetListTaskAccomplishedDoneOfTrainee(traineeId);

        //        int totalDoneTask = doneTasks.Count();

        //        int totalOverdueTask = totalTask - totalDoneTask;

        //        TaskCounterResponse result = new()
        //        {
        //            TotalTask = totalTask,
        //            TaskComplete = totalDoneTask,
        //            TaskOverdue = totalOverdueTask
        //        };
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task UpdatePoints(int trainerId, List<UpdateCriteriaRequest> requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==request.UserId);
                    if (user==null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND,"Trainee not found");
                    }    
                    foreach(var item in request.UserCriterias)
                    {
                        var usercriteria = await _unitOfWork.UserCriteriaRepository.GetFirst(c=>c.TemplateHeaderId==item.TemplateHeaderId && c.UserId==user.Id);
                        usercriteria.Point = item.Point;
                        usercriteria.TrainerIdMark = trainerId;
                        usercriteria.UpdatedDate= DateTimeService.GetCurrentDateTime();
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


        public async Task<List<UserCriteriaResponse>> GetCurrentUserCriteria(int tranerId, int ojtBatchId)
        {
            try
            {
                var users = await _unitOfWork.UserRepository.Get(c => c.UserReferenceId == tranerId && c.OJTBatchId == ojtBatchId, "UserCriterias");
                
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
                            Point = c.Point,
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
    }
}
