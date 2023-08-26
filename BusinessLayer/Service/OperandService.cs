using DataAccessLayer.Commons.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class OperandService
    {
        private static Dictionary<string, List<OperandModel>> operandDictionary = new Dictionary<string, List<OperandModel>>();

        static OperandService()
        {
            operandDictionary["Attendance"] = new List<OperandModel>
            {
                new OperandModel { Key = "TotalAttendanceDay", Name = "Tổng số ngày làm việc", Description = "Tổng số ngày làm việc" },
                new OperandModel { Key = "TotalWorkingDaysOfOjtBatch", Name = "Tổng số ngày tối đa làm việc của batch", Description = "Miêu tả số ngày tối đa có thể làm được của thực tập sinh" },
                new OperandModel { Key = "FullHourWorkingDays", Name = "Tổng ngày làm đủ giờ", Description = "miêu tả" },
                new OperandModel { Key = "LackOfHourWorkingDays", Name = "Tổng ngày làm thiếu giờ", Description = "miêu tả" },
                new OperandModel { Key = "ExcitementByMonths", Name = "Độ sôi nổi theo tháng", Description = "miêu tả" }
            };

            operandDictionary["Skill"] = new List<OperandModel>
            {
                new OperandModel { Key = "SkillRating1Star", Name = "Tổng số kỹ năng 1 sao", Description = "tổng số kĩ năng 1 sao của thực tập sinh" },
                new OperandModel { Key = "SkillRating2Star", Name = "Tổng số kỹ năng 2 sao", Description = "tổng số kĩ năng 2 sao của thực tập sinh" },
                new OperandModel { Key = "SkillRating3Star", Name = "Tổng số kỹ năng 3 sao", Description = "tổng số kĩ năng 3 sao của thực tập sinh" },
                new OperandModel { Key = "SkillRating4Star", Name = "Tổng số kỹ năng 4 sao", Description = "tổng số kĩ năng 4 sao của thực tập sinh" },
                new OperandModel { Key = "SkillRating5Star", Name = "Tổng số kỹ năng 5 sao", Description = "tổng số kĩ năng 5 sao của thực tập sinh" },
                new OperandModel { Key = "InitialSkillCount", Name = "Tổng số skill ban đầu", Description = "miêu tả" },
                new OperandModel { Key = "NewlyLearnedSkills", Name = "Tổng số skill học được", Description = "miêu tả" },
                new OperandModel { Key = "InitialSkillPoints", Name = "Tổng số điểm kỹ năng ban đầu", Description = "miêu tả" },
                new OperandModel { Key = "AchievedSkillPoints", Name = "Tổng số điểm kỹ năng đạt được", Description = "miêu tả" }
            };

            operandDictionary["Certificate"] = new List<OperandModel>
            {
                new OperandModel { Key = "RequiredCourses", Name = "Tổng số course bắt buộc", Description = "miêu tả" },
                new OperandModel { Key = "CompletedCourses", Name = "Tổng số course hoàn thành", Description = "miêu tả" },
                new OperandModel { Key = "CompletedRequiredCourses", Name = "Tổng số course bắt buộc đã hoàn thành", Description = "miêu tả" },
                new OperandModel { Key = "CompletedOptionalCourses", Name = "Tổng số course optional đã hoàn thành", Description = "miêu tả" }
            };

            operandDictionary["Task"] = new List<OperandModel>
            {
                new OperandModel { Key = "TotalTask", Name = "Tổng task", Description = "miêu tả" },
                new OperandModel { Key = "TaskOverdue", Name = "Tổng task trễ hạn", Description = "miêu tả" },
                new OperandModel { Key = "TaskComplete", Name = "Tổng task làm đúng hạn", Description = "miêu tả" },
                new OperandModel { Key = "TaskFailed", Name = "Tổng số task thất bại", Description = "miêu tả" }
            };
        }
        public List<OperandModel> GetAllOperands()
        {
            List<OperandModel> allOperands = new List<OperandModel>();
            foreach (var operandsByKey in operandDictionary.Values)
            {
                allOperands.AddRange(operandsByKey);
            }
            return allOperands;
        }
        public List<OperandModel> GetOperandsByKey(string key)
        {
            if (operandDictionary.ContainsKey(key))
            {
                return operandDictionary[key];
            }
            return null;
        }
    }
}
