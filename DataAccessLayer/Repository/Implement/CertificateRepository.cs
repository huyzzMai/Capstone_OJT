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
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<Certificate> GetCertificateWithUserAndCourse(int userId, int couseId)
        {
            Certificate certificate = await _context.Certificates
                .Where(u => u.UserId == userId && u.CourseId == couseId && u.Status != CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT)
                .Include(u => u.User)
                .Include(u => u.Course)
                .FirstOrDefaultAsync();
            return certificate; 
        }

        public async Task<List<Certificate>> GetListCertificateOfTraineeWithUserAndCourse(int userId)
        {
            List<Certificate> list = await _context.Certificates
                .Where(u => u.UserId == userId && u.Status != CommonEnums.CERTIFICATE_STATUS.NOT_SUBMIT)
                .Include(u => u.User)
                .Include(u => u.Course)
                .OrderByDescending(u => u.SubmitDate)
                .ToListAsync();
            return list;    
        }
    }
}
