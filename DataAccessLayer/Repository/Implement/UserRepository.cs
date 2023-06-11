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
    public class UserRepository : IUserRepository
    {
        private readonly OJTDbContext dbContext;

        public UserRepository(OJTDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailAndDeleteIsFalse(string email)
        {
            User user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
            return user;
        }
    }
}
