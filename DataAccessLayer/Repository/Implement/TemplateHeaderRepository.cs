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
    }
}
