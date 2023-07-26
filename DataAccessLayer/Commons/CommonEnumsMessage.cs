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
            public const string CREATED = "NewCourseCreated";
            public const string UPDATED = "NewCourseUpdated";
            public const string DELETED = "NewCourseDeleted";
        }
        public class SKILL_SIGNALR_MESSAGE
        {
            public const string CREATED = "NewSkillCreated";
            public const string UPDATED = "NewSkillUpdated";
            public const string DELETED = "NewSkillDeleted";
        }
    }
}
