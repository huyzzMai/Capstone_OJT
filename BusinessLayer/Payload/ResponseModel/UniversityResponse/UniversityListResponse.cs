using BusinessLayer.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UniversityResponse
{
    public class UniversityListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ImgURL { get; set; }

        public int? Status { get; set; }
       
        public string JoinDate { get; set; }

        public int TotalBatches { get; set; }

        public int OjtTrainees { get; set;}

        public int OjtActiveTrainees { get; set; }
    }
}
