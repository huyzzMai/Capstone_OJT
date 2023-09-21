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
            public const string ASSIGNED = "Trainer assigned Course to Trainee. Load get notification for Trainee.";
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
            public const string ASSIGNED = "Trainees have been assigned to a Trainer";
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
            public const string UPDATE_FINISH = "Trainee has checked finish a Task on Trello. Please process it.";
            public const string UPDATE_PROCESS = "Update Task Processing for Trainee. Reload notification for trainee.";
        }

        public class CERTIFICATE_MESSAGE
        {
            public const string PROCESS_CERTIFICATE = "Trainer process Trainee certificate. Trainee reload notificattion and get certificate.";
            public const string UPDATE_PROCESS = "Update Certificate Processing for Trainee";
        }

        public class NOTIFICATION_MESSAGE
        {
            public const string UPDATE_NOTI = "Update Notification Read. Reload notification list.";
            public const string CREATE_NOTI = "Notification created. Reload notification list.";
        }

        public class TEMPLATE_SIGNALR_MESSAGE
        {
            public const string CREATED = "New Template Created";
            public const string UPDATED = "New Template Updated";
            public const string DELETED = "New Template Deleted";
        }
        public class TEMPLATEHEADER_SIGNALR_MESSAGE
        {
            public const string CREATED = "New Template Header Created";
            public const string UPDATED = "New Template Header Updated";
            public const string DELETED = "New Template Header Deleted";
        }
        public class UNIVERSITY_SIGNALR_MESSAGE
        {
            public const string CREATED = "New University Created";
            public const string UPDATED = "New University Updated";
            public const string DELETED = "New University Deleted";
        }

        public class POSITION_SIGNALR_MESSAGE
        {
            public const string CREATED = "New Position Created";
            public const string UPDATED = "New Position Updated";
            public const string DELETED = "New Position Deleted";
        }
    }
}
