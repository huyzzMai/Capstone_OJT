using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.CriteriaResponse
{
    public class UserCriteriaResponse
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RollNumber { get; set; }

        public List<CriteriaResponse> Criterias { get; set; } 

    }
}
