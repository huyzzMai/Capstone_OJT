using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Service.Implement;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Vml;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
        private User user;
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration _configuration;
        private readonly TaskCounterResponse _counter;
        private int TotalWorkingDaysPerMonth;
        private int WorkHoursRequired;
        public OperandsHandler(int userId, IUnitOfWork unitOfWork, IConfiguration configuration,TaskCounterResponse counter)
        {          
            _unitOfWork = unitOfWork; 
            _configuration = configuration;
            _counter= counter;
            GetUserInform(userId);
            GenerateConfigData();
        }
         void GenerateConfigData()
        {
            var config = _unitOfWork.ConfigRepository.Get().Result;
            TotalWorkingDaysPerMonth = (int)config.FirstOrDefault(c => c.Name == "Total Working Days Per Month").Value;
            WorkHoursRequired = (int)config.FirstOrDefault(c => c.Name == "Work Hours Required").Value;
        }
        void GetUserInform(int id)
        {
            var userinfo = _unitOfWork.UserRepository.GetFirst(c=>c.Id==id, "Position", "OJTBatch", "Attendances", "UserCriterias", "UserSkills","Certificates").Result;
            user = userinfo;
        }
        public int TotalTask()
        {
            return _counter.TotalTask;
        }
        public int TaskOverdue()
        {
            return _counter.TaskOverdue;
        }
        public int TaskComplete()
        {
            return _counter.TaskComplete;
        }
        public int TaskFailed()
        {
            return _counter.TaskFail;
        }
        public int TotalAttendanceDay()
        {        
            return user.Attendances.Count();
        }
        public int TotalWorkingDaysOfOjtBatch()
        {
           
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

            return workingDays+1;
        }
        public int FullHourWorkingDays()
        {
            var attend = user.Attendances.Where(c=>c.TotalTime.Value.TotalHours >= WorkHoursRequired);
            return attend.Count();
        }
        public int LackOfHourWorkingDays()
        {
            var attend = user.Attendances.Where(c => c.TotalTime.Value.TotalHours < WorkHoursRequired);
            return attend.Count();
        }
        public int TotalDayWorkingOfUserByMonth(DateTime date)
        {
            DateTime nextMonthStart = new DateTime(date.Year, date.Month + 1, 1);
            DateTime monthEnd = nextMonthStart.AddDays(-1);

            var days = user.Attendances
                .Where(c => c.PresentDate.HasValue && c.PresentDate.Value >= date && c.PresentDate.Value <= monthEnd)
                .Count();

            return days;
        }

        public double ExcitementByMonths()
        {

            DateTime startDay = (DateTime)user.OJTBatch.StartTime;

            DateTime endDay = (DateTime)user.OJTBatch.EndTime;

            double totalA = 0;
            int totalMonths = 0;

            DateTime currentMonthStart = startDay;

            DateTime nextMonthStart = new DateTime(startDay.Year, startDay.Month+1, 1);


            while (currentMonthStart < endDay)
            {
                DateTime monthEnd;

                if (currentMonthStart > endDay)
                {
                     monthEnd = endDay;
                }
                else
                {
                     monthEnd = nextMonthStart.AddDays(-1);
                }
                
                int daysInMonth = DateTimeService.GetTotalDaysInMonth(currentMonthStart.Year,currentMonthStart.Month);

                int daysRemainingInMonth = (monthEnd - currentMonthStart).Days+1;

                double workingday = TotalDayWorkingOfUserByMonth(currentMonthStart);
                double b = daysRemainingInMonth / daysInMonth * TotalWorkingDaysPerMonth;
                double a = workingday / b;

                totalA += a;

                totalMonths++;

                currentMonthStart = nextMonthStart;

                nextMonthStart = currentMonthStart.AddMonths(1);
            }

            double averageA = totalA / (double)totalMonths;

            return averageA;
        }

        public int InitialSkillCount()
        {
            int count = user.UserSkills.Where(c=>c.InitLevel != 0).Count();
            return count;
        }

        public int NewlyLearnedSkills()
        {
            int count = user.UserSkills.Where(c => c.InitLevel == 0).Count();
            return count;
        }

        public int? InitialSkillPoints()
        {
            int? count = user.UserSkills.Where(c => c.InitLevel != 0).Sum(c => c.InitLevel);
            return count;
        }

        public int? AchievedSkillPoints()
        {
            int? count = user.UserSkills.Sum(c => c.CurrentLevel) - InitialSkillPoints();
            return count;
        }

        public int SkillRating1Star()
        {
            int count = user.UserSkills.Where(c => c.CurrentLevel == 1).Count();
            return count;
        }

        public int SkillRating2Star()
        {
            int count = user.UserSkills.Where(c => c.CurrentLevel == 2).Count();
            return count;
        }

        public int SkillRating3Star()
        {
            int count = user.UserSkills.Where(c => c.CurrentLevel == 3).Count();
            return count;
        }

        public int SkillRating4Star()
        {
            int count = user.UserSkills.Where(c => c.CurrentLevel == 4).Count();
            return count;
        }

        public int SkillRating5Star()
        {
            int count = user.UserSkills.Where(c => c.CurrentLevel == 5).Count();
            return count;
        }

        public int RequiredCourses()
        {
           var cer = _unitOfWork.CertificateRepository.Get(c=>c.UserId==user.Id 
           && c.Course.CoursePositions.Any(c=>c.IsCompulsory==true),"Course").Result;
            return cer.Count();
        }
        public int CompletedCourses()
        {
            var cer = _unitOfWork.CertificateRepository.Get(c => c.UserId == user.Id 
            && c.Status==CommonEnums.CERTIFICATE_STATUS.APPROVED).Result;
            return cer.Count();
        }

        public int CompletedRequiredCourses()
        {
            var cer = _unitOfWork.CertificateRepository.Get(c => c.UserId == user.Id
            && c.Course.CoursePositions.Any(c => c.IsCompulsory == true) 
            && c.Status == CommonEnums.CERTIFICATE_STATUS.APPROVED, "Course").Result;
            return cer.Count();
        }

        public int CompletedOptionalCourses()
        {
            var cer = _unitOfWork.CertificateRepository.Get(c => c.UserId == user.Id
            && c.Course.CoursePositions.Any(c => c.IsCompulsory == false)
            && c.Status == CommonEnums.CERTIFICATE_STATUS.APPROVED, "Course").Result;
            return cer.Count();
        }
    }
}
