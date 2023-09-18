using BusinessLayer.Payload.ResponseModel.CourseResponse;
using BusinessLayer.Payload.ResponseModel.OJTBatchResponse;
using BusinessLayer.Utilities;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.UniversityResponse
{
    public class UniversityDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ImgURL { get; set; }

        public int? Status { get; set; }

        
        public string JoinDate { get; set; }

        public string UniversityCode { get; set; }
       
        public string CreatedAt { get; set; }

        
        public string UpdatedAt { get; set; }

        public List<ValidOJTBatchResponse> validOJTBatchResponses { get; set; }
    }
}
