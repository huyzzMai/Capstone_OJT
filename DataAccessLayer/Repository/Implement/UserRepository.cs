using DataAccessLayer.Base;
using DataAccessLayer.Commons;
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<User> GetUserByEmailAndDeleteIsFalse(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAndDeleteIsFalse(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
            return user;
        }

        public async Task<User> GetUserByResetCode(string token)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPassordCode == token);
            return user;
        }

        public async Task<User> GetUserByResetCodeAndDeleteIsFalse(string token)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPassordCode == token && u.IsDeleted == false);
            return user;
        }

        public async Task<List<User>> GetTraineeList()
        {
            List<User> users = await _context.Users
                .Where(u => u.IsDeleted == false && u.Role == CommonEnums.ROLE.TRAINEE)
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> GetTrainerList()
        {
            List<User> users = await _context.Users
                .Where(u => u.IsDeleted == false && u.Role == CommonEnums.ROLE.TRAINER)
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> GetTraineeListByTrainerId(int id)
        {
            List<User> users = await _context.Users
                .Where(u => u.IsDeleted == false && u.Role == CommonEnums.ROLE.TRAINEE && u.UserReferenceId==id)
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> GetTraineeListByBatch(int batchid)
        {
            List<User> users = await _context.Users
                .Where(u => u.IsDeleted == false && u.Role == CommonEnums.ROLE.TRAINEE && u.OJTBatchId==batchid)
                .ToListAsync();
            return users;
        }
    }
}
