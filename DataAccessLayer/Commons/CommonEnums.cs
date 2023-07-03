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
        
        public class DataExcel
        {
            public const string TraineeName = "Name";
            public const string NumberStudent = "RollNumber";
            public const string Gender = "Gender";
            public const string PhoneNumber = "PhoneNumber";
            public const string Address = "Address";
            public const string Email = "Email";
            public const string Position = "Position";         
        }
        //public class DEPARTMENTID
        //{
        //    public const int ADMINDEPARTMENT = 1;
        //    public const int MAINTENANCEDEPARTMENT = 2;
        //    public const int USERDEPARTMENT = 3;
        //    public const int FACILITYDEPARTMENT = 4;
        //}
    }
}
