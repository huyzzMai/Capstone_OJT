using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class CertificateRepository : GenericRepository<Registration>, ICertificateRepository
    {
        public CertificateRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<Registration> GetCertificateWithUserAndCourse(int userId, int couseId)
        {
            Registration certificate = await _context.Certificates
                .Where(u => u.UserId == userId && u.CourseId == couseId && u.Status != CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT)
                .Include(u => u.User)
                .Include(u => u.Course)
                .FirstOrDefaultAsync();
            return certificate; 
        }

        public async Task<List<Registration>> GetListCertificateOfTraineeWithUserAndCourse(int userId)
        {
            List<Registration> list = await _context.Certificates
                .Where(u => u.UserId == userId && u.Status != CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT)
                .Include(u => u.User)
                .Include(u => u.Course)
                .OrderByDescending(u => u.SubmitDate)
                .ToListAsync();
            return list;    
        }
    }
}
