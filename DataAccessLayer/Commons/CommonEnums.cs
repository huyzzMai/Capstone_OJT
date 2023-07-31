using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Commons
{
    public static class CommonEnums
    {
        public class ROLE
        {
            public const int ADMIN = 1;
            public const int MANAGER = 2;
            public const int TRAINER = 3;
            public const int TRAINEE = 4;
        }

        public class USER_GENDER
        {
            public const int MALE = 1;
            public const int FEMALE = 2;
            public const int OTHER = 3;
        }

        public class USER_STATUS
        {
            public const int DELETED = 1;
            public const int ACTIVE = 2;
        }

        public class TRAINING_PLAN_STATUS
        {
            public const int DELETED = 1;
            public const int PENDING = 2;
            public const int ACTIVE = 3;
            public const int DENIED = 4;
            public const int CLOSED = 5;
        }

        public class TRAINING_PLAN_DETAIL_STATUS
        {
            public const int DELETED = 1;
            public const int ACTIVE = 2;
            public const int CLOSED = 3;    
        }

        public class TRAINEE_TASK_STATUS
        {
            public const int FINISHED = 1;
            public const int OVERDUE = 2;
            public const int IN_PROCESS = 3;
        }

        public class ACCOMPLISHED_TASK_STATUS
        {
            public const int PENDING = 1;
            public const int DONE = 2;
            public const int FAILED = 3;
        }

        public class TEMPLATE_STATUS
        {
            public const int DELETED = 1;
            public const int ACTIVE = 2;
            public const int CLOSED = 3;
        }

        public class COURSE_STATUS
        {
            public const int DELETED = 1;
            public const int ACTIVE = 2;
            public const int CLOSED = 3;
        }

        public class SKILL_STATUS
        {
            public const int DELETED = 1;
            public const int ACTIVE = 2;
            public const int CLOSED = 3;
        }

        public class SKILL_LEVEL
        {
            public const int BEGINNER = 1;
            public const int ELEMENTARY = 2;
            public const int INTERMEDIATE = 3;
            public const int ADVANCED = 4;
            public const int FLUENT = 5;
        }
        public class CERTIFICATE_STATUS
        {
            public const int DELETED = 1;
            public const int PENDING = 2;
            public const int NOT_SUBMIT = 3;
            public const int APPROVED = 4;
            public const int DENY = 5;
        }
        public class POSITION
        {
            public const int BACKEND_DEVELOPER = 1;
            public const int FRONTEND_DEVELOPER = 2;
            public const int BUSINESS_ANALYST = 3;
            public const int DESIGN_UX_UI = 4;
            public const int TESTER = 5;
        }

        public class  CLIENT_ERROR
        {
            public const int BAD_REQUET = 400;
            public const int UNAUTHORIZED = 401;
            public const int NOT_FOUND = 404;
            public const int CONFLICT = 409;
        }      
    }      
}

