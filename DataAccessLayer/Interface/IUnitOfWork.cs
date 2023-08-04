using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITemplateHeaderRepository TemplateHeaderRepository { get; }
        ITrainingPlanRepository TrainingPlanRepository { get; } 
        ITrainingPlanDetailRepository TrainingPlanDetailRepository { get; } 
        IUserTrainingPlanRepository UserTrainingPlanRepository { get; }
        IOJTBatchRepository OJTBatchRepository { get; }
        ITaskRepository TaskRepository { get; }

        ICourseRepository CourseRepository { get; }

        ICoursePositionRepository CoursePositionRepository { get; }

        ICertificateRepository CertificateRepository { get; }

        IUserSkillRepository UserSkillRepository { get; }

        ICourseSkillRepository CourseSkillRepository { get; }

        ISkillRepository SkillRepository { get; }

        INotificationRepository NotificationRepository { get; } 

        IUniversityRepository UniversityRepository { get; }

        ITemplateRepository TemplateRepository { get; }

        IUserCriteriaRepository UserCriteriaRepository { get; } 

        IAttendanceRepository AttendanceRepository { get; }
    }
}
