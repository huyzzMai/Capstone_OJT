using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Commons.CommonModels
{
    public class UserCriteriaReport
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string RollNumber { get; set; }

        public DateTime Birthday { get; set; }

        public string Position { get; set; }

        public string University { get; set; }

        public virtual ICollection<TemplatePoint>? TemplatePoint { get; set; }

    }
}
