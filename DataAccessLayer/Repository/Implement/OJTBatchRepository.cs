using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class OJTBatchRepository : GenericRepository<OJTBatch>, IOJTBatchRepository
    {
        public OJTBatchRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<OJTBatch>> Get(Expression<Func<OJTBatch, bool>> expression = null, params string[] includeProperties)
        {
            var result = await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.Trainees = _unitOfWork.UserRepository.Get(c => c.OJTBatchId == item.Id, includeProperties: "UserCriterias").Result.ToList();             
            }
            return result;
        }

        public async Task<List<OJTBatch>> GetlistOjtbatchWithFormula(int formulaId)
        {
            var listojt = await _context.OJTBatches
                .Where(o=>o.Template.TemplateHeaders.Any(th=>th.FormulaId==formulaId) && o.EndTime.Value.AddDays(10) > DateTime.UtcNow.AddHours(7))
                .Include(c => c.Template)
                .ThenInclude(c => c.TemplateHeaders)
                .ToListAsync();
            return listojt;
        }
    }
}
