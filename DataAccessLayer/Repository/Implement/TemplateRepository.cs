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
    public class TemplateRepository : GenericRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Template>> Get(Expression<Func<Template, bool>> expression = null, params string[] includeProperties)
        {
            var result= await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.TemplateHeaders = _unitOfWork.TemplateHeaderRepository.Get(c => c.TemplateId == item.Id, includeProperties: "UserCriterias").Result.ToList();
            }
            return result;

        }
    }
}
