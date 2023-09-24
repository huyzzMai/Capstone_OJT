using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.ChartResponse
{
    public class TopSkillTraineeResponse
    {
        public string SkillName { get; set; }   
        public int InitSkill { get; set; }
        public int CurrentSkill { get; set;}
    }
}
