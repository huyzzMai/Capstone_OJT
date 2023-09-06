using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ICertificateRepository : IGenericRepository<Registration>
    {
        Task<Registration> GetCertificateWithUserAndCourse(int userId, int couseId);
        Task<List<Registration>> GetListCertificateOfTraineeWithUserAndCourse(int userId);
    }
}
