using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Commons.CommonModels;
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
    public class TemplateHeaderRepository : GenericRepository<TemplateHeader>, ITemplateHeaderRepository
    {
        public TemplateHeaderRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context,unitOfWork)
        {
        }
        //public async Task<List<TemplatePoint>> GetPointListByUserId(int id)
        //{
        //    var list = new List<TemplatePoint>();
        //    var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == id && c.Status == CommonEnums.USER_STATUS.ACTIVE, "OJTBatch");
        //    var listtemplate = await _context.Templates.Where(c => c.UniversityId == user.OJTBatch.UniversityId && c.Status == CommonEnums.TEMPLATE_STATUS.ACTIVE).Include(c => c.TemplateCriterias).ToListAsync();
        //    foreach (var item in listtemplate)
        //    {
               
        //        var Criteria = new TemplatePoint()
        //        {
        //            Name = item.Name,
        //            Point =  await GetPointByTemplateIdandUserid(user.Id,item.Id)
        //    };
        //        list.Add(Criteria);
        //    }
        //    return list;
        //}

        //public async Task<int?> GetPointByTemplateIdandUserid(int userid, int templateid)
        //{
        //    int? point = null;
        //    var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == userid && c.Status == CommonEnums.USER_STATUS.ACTIVE, "OJTBatch");
        //    var tempelate = await _context.Templates.Where(c => c.Id == templateid && c.Status == CommonEnums.TEMPLATE_STATUS.ACTIVE).Include(c => c.TemplateCriterias).FirstOrDefaultAsync();
        //    if(tempelate==null || tempelate.TemplateCriterias == null) 
        //    {
        //        return null;
        //    }
        //    foreach (var item in tempelate.TemplateCriterias)
        //    {
        //        var userCriteria = await _context.UserCriterias.Where(c => c.UserId == user.Id && c.CriteriaId == item.CriteriaId).FirstOrDefaultAsync();
        //        if (userCriteria != null && userCriteria.Point != null)
        //        {
        //            point = userCriteria.Point;
        //            break; 
        //        }
        //    }
        //    return point;
        //}     
    }
}
