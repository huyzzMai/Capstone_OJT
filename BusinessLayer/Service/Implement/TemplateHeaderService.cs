using BusinessLayer.Models.RequestModel.CriteriaRequest;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TemplateHeaderService : ITemplateHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TemplateHeaderService(IUnitOfWork unitOfWork) 
         {
             _unitOfWork = unitOfWork;
         }
        public List<string> GetPropertyData(List<User> userList, string propertyName)
        {
            var propertyType = typeof(User).GetProperty(propertyName)?.PropertyType;

            if (propertyType == null || propertyType != typeof(string))
            {
                throw new ArgumentException($"Property '{propertyName}' is not of type string in the User class.");
            }

            var values = userList.Select(user => user.GetType().GetProperty(propertyName)?.GetValue(user)?.ToString())
                                 .Where(value => value != null)
                                 .ToList();
            return values;
        }
        //public async Task<Criteria> CreateCriteria(CreateCriteriaRequest request)
        //{
        //    var criteria= new Criteria();
        //    criteria.Name = request.Name;
        //    criteria.TotalPoint= request.TotalPoint;
        //    criteria.CreatedAt=DateTime.Now;
        //    //criteria.IsDeleted = false;
        //    await _unitOfWork.CriteriaRepository.Add(criteria);
        //    return criteria;
        //}
    }
}
