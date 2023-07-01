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

        public class USER_STATUS
        {
            public const int ACTIVE = 1;
        }

        public class TRAINING_PLAN_STATUS
        {
            public const int PENDING = 1;
            public const int ACCEPTED = 2;
            public const int DENIED = 3;
        }
    }
}
