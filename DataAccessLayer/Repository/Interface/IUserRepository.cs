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
        Task<User> GetUserByEmailAndDeleteIsFalse(string email);

        Task<User> GetUserByIdAndDeleteIsFalse(int id);

        Task<List<User>> GetTrainerList();

        Task<List<User>> GetTraineeList();

        Task<List<User>> GetTraineeListByTrainerId(int id);
    }
}
