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

        Task<User> GetUserByRefTokenAndStatusActive(string token); 

        Task<User> GetUserByTrelloIdAndStatusActive(string id);

        Task<User> GetUserByIdWithSkillList(int id);

        Task<List<User>> GetTrainerList(string keyword, int? position);

        Task<User> GetTrainerWithListTraineeByIdAndStatusActive(int id);

        Task<List<User>> GetUnassignedTraineeList();

        Task<List<User>> GetTraineeList(string keyword, int? position);

        Task<List<User>> GetTraineeListByBatch(int batchid);

        Task<List<User>> GetTraineeListByTrainerId(int id);

        Task<List<UserSkill>> GetListUserSkillTrainee(int userId);

        Task<List<User>> GetListTraineeByTrainerIdWithTaskAccomplishedList(int userId);

        //Task<List<UserCriteriaReport>> GetUserReportList(int batchid,List<User> user);
    }
}
