﻿using System;
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
            public const int DELETED = 0;
            public const int ACTIVE = 1;
        }

        public class TRAINING_PLAN_STATUS
        {
            public const int DELETED = 0;
            public const int PENDING = 1;
            public const int ACTIVE = 2;
            public const int DENIED = 3;
            public const int CLOSED = 4;
        }

        public class TRAINING_PLAN_DETAIL_STATUS
        {
            public const int DELETED = 0;
            public const int ACTIVE = 1;
            public const int CLOSED = 2;    
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
            public const int DELETED = 0;
            public const int ACTIVE = 1;
            public const int CLOSED = 2;
        }

        public class COURSE_STATUS
        {
            public const int DELETED = 0;
            public const int ACTIVE = 1;
            public const int CLOSED = 2;
        }

        public class SKILL_STATUS
        {
            public const int DELETED = 0;
            public const int ACTIVE = 1;
            public const int CLOSED = 2;
        }

        public class SKILL_LEVEL
        {
            public const int BEGINNER = 0;
            public const int ELEMENTARY = 1;
            public const int INTERMEDIATE = 2;
            public const int ADVANCED = 3;
            public const int FLUENT = 4;
        }
        public class CERTIFICATE_STATUS
        {
            public const int DELETED = 0;
            public const int PENDING = 1;
            public const int NOT_SUBMIT = 2;
            public const int APPROVED = 3;
            public const int DENY = 4;
        }
        public class POSITION
        {
            public const int BACKEND_DEVELOPER = 0;
            public const int FRONTEND_DEVELOPER = 1;
            public const int BUSINESS_ANALYST = 2;
            public const int DESIGN_UX_UI = 3;
            public const int TESTER = 4;
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

