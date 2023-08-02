using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.ResponseModel.UniversityResponse
{
    public class UniversityListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ImgURL { get; set; }

        public int? Status { get; set; }

        public DateTime? JoinDate { get; set; }

        public int TotalBatches { get; set; }

        public int OjtTrainees { get; set;}

        public int OjtActiveTrainees { get; set; }
    }
}
