using BusinessLayer.Models.ResponseModel.OJTBatchResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class OJTBatchService : IOJTBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OJTBatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtList()
        {
            var list = await _unitOfWork.OJTBatchRepository.Get(c=>c.IsDeleted==false && c.EndTime > DateTime.Now);
            if (list == null)
            {
                throw new Exception("Empty List OJTBatch");             
            }
            IEnumerable<ValidOJTBatchResponse> res = list.Select(
                ojt =>
                {
                    return new ValidOJTBatchResponse()
                    {
                        Id =ojt.Id,
                        Name = ojt.Name  ,
                        StartTime = ojt.StartTime,
                        EndTime = ojt.EndTime
                    };
                }
                ).ToList();
            return res;
        }
        public async Task<IEnumerable<ValidOJTBatchResponse>> GetValidOJtListbyUniversityId(int id)
        {
            var list = await _unitOfWork.OJTBatchRepository.Get(c => c.IsDeleted == false && c.UniversityId == id);
            if (list == null)
            {
                throw new Exception("Empty List OJTBatch");
            }
            IEnumerable<ValidOJTBatchResponse> res = list.Select(
                ojt =>
                {
                    return new ValidOJTBatchResponse()
                    {
                        Id = ojt.Id,
                        Name = ojt.Name,
                        StartTime = ojt.StartTime,
                        EndTime = ojt.EndTime
                    };
                }
                ).ToList();
            return res;
        }
    }
}
