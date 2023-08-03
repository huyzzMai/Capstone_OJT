using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Commons
{
    public static class CommonEnumsMessage
    {
        public class COURSE_SIGNALR_MESSAGE
        {
            public const string CREATED = "New Course Created";
            public const string UPDATED = "New Course Updated";
            public const string DELETED = "New Course Deleted";
        }

        public class SKILL_SIGNALR_MESSAGE
        {
            public const string CREATED = "New Skill Created";
            public const string UPDATED = "New Skill Updated";
            public const string DELETED = "New Skill Deleted";
        }

        public class USER_MESSAGE
        {
            public const string CREATE = "New User Created";
            public const string UPDATE = "User Profile Updated";
        }

        public class TRAINING_PLAN_MESSAGE
        {
            public const string CREATE = "New Training Plan Created";
            public const string UPDATE = "Training Plan Updated";
            public const string DELETE = "Training Plan Deleted";
            public const string DETAIL_DELETE = "Detail of training plan Deleted";
            public const string ASSIGN = "Training plan assigned to trainee";
            public const string PROCESS = "Training plan has been process. Please reload get training plan and get notification for user.";
        }

        public class TASK_MESSAGE
        {
            public const string UPDATE_FINISH = "Update Task Finished for Trainee";
            public const string UPDATE_PROCESS = "Update Task Processing for Trainer";
        }

        public class CERTIFICATE_MESSAGE
        {
            public const string PROCESS_CERTIFICATE = "Trainer process Trainee certificate. Trainee reload notificattion and get certificate.";
            public const string UPDATE_PROCESS = "Update Task Processing for Trainer";
        }

        public class NOTIFICATION_MESSAGE
        {
            public const string UPDATE_NOTI = "Update Notification Read. Reload notification list.";
        }
    }
}
