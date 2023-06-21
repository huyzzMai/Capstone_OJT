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
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
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
    }
}
