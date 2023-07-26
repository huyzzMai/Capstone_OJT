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

        public class TRAINING_PLAN_MESSAGE
        {
            public const string CREATE = "New Training Plan Created";
            public const string UPDATE = "Training Plan Updated";
            public const string DELETE = "Training Plan Delete";
        }
    }
}
