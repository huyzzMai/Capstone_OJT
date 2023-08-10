using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrelloDotNet;

namespace BusinessLayer.Utilities
{
    public class OperandsHandler
    {     
        private readonly int userId;
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration _configuration;
        private readonly TaskCounterResponse _counter;
        public OperandsHandler(int userId, IUnitOfWork unitOfWork, IConfiguration configuration,TaskCounterResponse counter)
        {
            this.userId = userId;
            _unitOfWork = unitOfWork; 
            _configuration = configuration;
            _counter= counter;
        }
        public int TotalTask()
        {
            //try
            //{
            //    var user =  _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId).Result;
            //    if (user == null)
            //    {
            //        throw new Exception("User not found!");
            //    }
            //    var trelloUserId = user.TrelloId;

            //    var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

            //    var cards =  client.GetCardsForMemberAsync(trelloUserId).Result.ToList();

            //    int totalTask = cards.Count();

            //    return totalTask;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return _counter.TotalTask;
        }
        public int TaskOverdue()
        {
            //try
            //{
            //    var user =  _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId).Result;
            //    if (user == null)
            //    {
            //        throw new Exception("User not found!");
            //    }
            //    var trelloUserId = user.TrelloId;
            //    var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

            //    var cards = client.GetCardsForMemberAsync(trelloUserId).Result.ToList();
            //    int totalTask = cards.Count();

            //    var doneTasks = _unitOfWork.TaskRepository.GetListTaskAccomplishedDoneOfTrainee(userId).Result.ToList();

            //    int totalDoneTask = doneTasks.Count();

            //    int totalOverdueTask = totalTask - totalDoneTask;

            //    return totalOverdueTask;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return _counter.TaskOverdue;
        }
        public int TaskComplete()
        {
            //try
            //{
            //    var user = _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId).Result;
            //    if (user == null)
            //    {
            //        throw new Exception("User not found!");
            //    }
            //    var trelloUserId = user.TrelloId;
            //    var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

            //    var cards = client.GetCardsForMemberAsync(trelloUserId).Result.ToList();
            //    int totalTask = cards.Count();

            //    var doneTasks = _unitOfWork.TaskRepository.GetListTaskAccomplishedDoneOfTrainee(userId).Result.ToList();

            //    int totalDoneTask = doneTasks.Count();

            //    return totalDoneTask;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return _counter.TaskOverdue;
        }
        public int TotalAttendanceDay()
        {
            var tem =  _unitOfWork.AttendanceRepository.Get(c=>c.UserId==userId).Result;
            return tem.Count();
        }
        public int CalculateWorkingDays()
        {
            var user = _unitOfWork.UserRepository.GetFirst(c => c.Id == userId,"OJTBatch").Result;
            DateTime? startTime = (DateTime)user.OJTBatch.StartTime;
            DateTime? endTime = (DateTime)user.OJTBatch.EndTime;

            if (!startTime.HasValue || !endTime.HasValue || startTime > endTime)
            {
                return 0;
            }
            int workingDays = 0;
            DateTime currentDate = startTime.Value.Date;
            while (currentDate <= endTime.Value.Date)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return workingDays;
        }
       
    }
}
