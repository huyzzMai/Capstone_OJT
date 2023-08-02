using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class UniversityRepository : GenericRepository<University>,IUniversityRepository
    {
        public UniversityRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<University>> Get(Expression<Func<University, bool>> expression = null, params string[] includeProperties)
        {
            var result= await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.OJTBatches = _unitOfWork.OJTBatchRepository.Get(c => c.Id == item.Id,"Trainees").Result.ToList();
            }
            return result;
        }
    }
}
