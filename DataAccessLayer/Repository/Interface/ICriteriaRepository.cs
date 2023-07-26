using DataAccessLayer.Commons.CommonModels;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ICriteriaRepository : IGenericRepository<TemplateHeader>
    {
        //Task<List<TemplatePoint>> GetPointListByUserId(int id);
    }
}
