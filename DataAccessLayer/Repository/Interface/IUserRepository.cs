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
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAndStatusActive(string email);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByIdAndStatusActive(int id);

        Task<User> GetUserByResetCode(string token);

        Task<User> GetUserByResetCodeAndStatusActive(string token);

        Task<List<User>> GetTrainerList();

        Task<List<User>> GetTraineeList();

        Task<List<User>> GetTraineeListByBatch(int batchid);
        Task<List<User>> GetTraineeListByTrainerId(int id);

        Task<List<UserCriteriaReport>> GetUserReportList(int batchid,List<User> user);
    }
}
