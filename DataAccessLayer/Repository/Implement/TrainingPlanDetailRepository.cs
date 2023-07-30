using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class TrainingPlanDetailRepository : GenericRepository<TrainingPlanDetail>, ITrainingPlanDetailRepository
    {
        public TrainingPlanDetailRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        { }
    }
}
