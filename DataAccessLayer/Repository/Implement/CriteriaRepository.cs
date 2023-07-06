using DataAccessLayer.Base;
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
    public class CriteriaRepository : GenericRepository<Criteria>, ICriteriaRepository
    {
        public CriteriaRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context,unitOfWork)
        {
        }

        public async Task<List<TemplatePoint>> GetPointListByUserId(int id)
        {
           var list = new List<TemplatePoint>();
           var listcriteriauser = await _context.UserCriterias.Where(c=>c.UserId==id).OrderBy(c=>c.CriteriaId).ToListAsync();
            foreach (var item in listcriteriauser)
            {
                var temp = await _context.TemplateCriterias.Where(c => c.CriteriaId == item.CriteriaId).Include(c=>c.Template).FirstOrDefaultAsync();
                var Criteria = new TemplatePoint()
                {
                    Name = temp.Template.Name,
                    Point = item.Point
                };
                list.Add(Criteria);
            }
            return list;
        }
    }
}
