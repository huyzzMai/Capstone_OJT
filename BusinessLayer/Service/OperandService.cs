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
                new OperandModel { Key = "TotalAttendanceDay", Name = "Tổng số ngày làm việc", 
                    Description = "Miêu tả tổng số ngày làm việc ghi nhận được của user " },
                new OperandModel { Key = "TotalWorkingDaysOfOjtBatch", Name = "Tổng số ngày tối đa làm việc của batch", 
                    Description = "Miêu tả tổng số ngày làm ước tính có thể đạt trong ojt batch mà user đó tham gia" },
                new OperandModel { Key = "FullHourWorkingDays", Name = "Tổng ngày làm đủ giờ", 
                    Description = "Miêu tả số lượng ngày được ghi nhận trong hệ thống mà user đó làm đủ giờ theo quy định" },
                new OperandModel { Key = "LackOfHourWorkingDays", Name = "Tổng ngày làm thiếu giờ", 
                    Description = "Miêu tả số lượng ngày được ghi nhận trong hệ thống mà user đó làm thiếu giờ theo quy định" },
                new OperandModel { Key = "ExcitementByMonths", Name = "Độ sôi nổi theo tháng",
                    Description = "Miêu tả trung bình độ chuyên cần của user theo các tháng dựa trên số ngày đi làm của các tháng được hệ thống ghi nhận" }
            };

            operandDictionary["Skill"] = new List<OperandModel>
            {
                new OperandModel { Key = "SkillRating1Star", Name = "Tổng số kỹ năng 1 sao", 
                    Description = "Miêu tả tổng số kĩ năng 1 sao của thực tập sinh đó đạt được theo thời điểm hiện tại hệ thống ghi nhận được" },
                new OperandModel { Key = "SkillRating2Star", Name = "Tổng số kỹ năng 2 sao", 
                    Description = "Miêu tả tổng số kĩ năng 2 sao của thực tập sinh đó đạt được theo thời điểm hiện tại hệ thống ghi nhận được" },
                new OperandModel { Key = "SkillRating3Star", Name = "Tổng số kỹ năng 3 sao", 
                    Description = "Miêu tả tổng số kĩ năng 3 sao của thực tập sinh đó đạt được theo thời điểm hiện tại hệ thống ghi nhận được" },
                new OperandModel { Key = "SkillRating4Star", Name = "Tổng số kỹ năng 4 sao", 
                    Description = "Miêu tả tổng số kĩ năng 4 sao của thực tập sinh đó đạt được theo thời điểm hiện tại hệ thống ghi nhận được" },
                new OperandModel { Key = "SkillRating5Star", Name = "Tổng số kỹ năng 5 sao", 
                    Description = "Miêu tả tổng số kĩ năng 5 sao của thực tập sinh đó đạt được theo thời điểm hiện tại hệ thống ghi nhận được" },
                new OperandModel { Key = "InitialSkillCount", Name = "Tổng số skill ban đầu", 
                    Description = "Miêu tả số lượng kĩ năng khởi tạo ban đầu thực tập sinh có" },
                new OperandModel { Key = "NewlyLearnedSkills", Name = "Tổng số skill học được", 
                    Description = "Miêu tả số lượng kĩ năng mới thực tập sinh đạt được so với thời điểm ban đầu" },
                new OperandModel { Key = "InitialSkillPoints", Name = "Tổng số điểm kỹ năng ban đầu", 
                    Description = "Miêu tả tổng số lượng level kĩ năng ban đầu của thực tập sinh" },
                new OperandModel { Key = "AchievedSkillPoints", Name = "Tổng số điểm kỹ năng đạt được", 
                    Description = "Miêu tả tổng số lượng level kĩ năng hiện tại của thực tập sinh" }
            };

            operandDictionary["Certificate"] = new List<OperandModel>
            {
                new OperandModel { Key = "RequiredCourses", Name = "Tổng số course bắt buộc", 
                    Description = "Miêu tả số lượng tổng số course bắt buộc của thực tập sinh" },
                new OperandModel { Key = "CompletedCourses", Name = "Tổng số course hoàn thành", 
                    Description = "Miêu tả số lượng tổng số course hoàn thành của thực tập sinh" },
                new OperandModel { Key = "CompletedRequiredCourses", Name = "Tổng số course bắt buộc đã hoàn thành", 
                    Description = "Miêu tả số lượng tổng số course bắt buộc đã hoàn thành của thực tập sinh" },
                new OperandModel { Key = "CompletedOptionalCourses", Name = "Tổng số course optional đã hoàn thành", 
                    Description = "Miêu tả số lượng tổng số course tự chọn đã hoàn thành của thực tập sinh" }
            };

            operandDictionary["Task"] = new List<OperandModel>
            {
                new OperandModel { Key = "TotalTask", Name = "Tổng task", 
                    Description = "Miêu tả tổng số task của thực tập sinh có" },
                new OperandModel { Key = "TaskOverdue", Name = "Tổng task trễ hạn", 
                    Description = "Miêu tả tổng số task của thực tập sinh bị hoàn thành trễ hẹn" },
                new OperandModel { Key = "TaskComplete", Name = "Tổng task được chấp thuận", 
                    Description = "Miêu tả tổng số task của thực tập sinh đã hoàn thành được đào tạo viện chấp thuận" },
                new OperandModel { Key = "TaskFailed", Name = "Tổng số task thất bại", 
                    Description = "Miêu tả tổng số task của thực tập sinh đã hoàn thành bị đào tạo viện không chấp thuận" }
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
