using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.CertificateResponse
{
    public class TrainerCertificateResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseImg { get; set; }
        public DateTime EnrollDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public string LinkCertificate { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AvatarURL { get; set; }
    }
}
