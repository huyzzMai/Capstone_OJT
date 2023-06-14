using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class CriteriaService : ICriteriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CriteriaService(IUnitOfWork unitOfWork) 
         {
             _unitOfWork = unitOfWork;
         }    
        public async Task<Criteria> CreateCriteria(CreateCriteriaRequest request)
        {
            var criteria= new Criteria();
            criteria.Name = request.Name;
            criteria.TotalPoint= request.TotalPoint;
            criteria.CreatedAt=DateTime.Now;
            criteria.IsDeleted = false;
            await _unitOfWork.CriteriaRepository.Add(criteria);
            return criteria;
        }
    }
}
