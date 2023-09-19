using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Commons.CommonModels;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<User>> Get(Expression<Func<User, bool>> expression = null, params string[] includeProperties)
        {
            var result = await base.Get(expression, includeProperties);
            foreach (var item in result)
            {
                item.UserCriterias = _unitOfWork.UserCriteriaRepository.Get(c => c.UserId == item.Id, "TemplateHeader").Result.ToList();
            }
            return result;
        }
        public async Task<User> GetUserByEmailAndStatusActive(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == CommonEnums.USER_STATUS.ACTIVE);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAndStatusActive(int id)
        {
            User user = await _context.Users
                                      .Where(u => u.Id == id && u.Status == CommonEnums.USER_STATUS.ACTIVE)
                                      .Include(u => u.Position)
                                      .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByResetCode(string token)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPassordCode == token);
            return user;
        }

        public async Task<User> GetUserByResetCodeAndStatusActive(string token)
        {
            User user = await _context.Users
                .Where(u => u.ResetPassordCode == token && u.Status == CommonEnums.USER_STATUS.ACTIVE)
                .Include(u => u.Position)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByRefTokenAndStatusActive(string token)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token && u.Status == CommonEnums.USER_STATUS.ACTIVE);
            return user;
        }

        public async Task<User> GetUserByTrelloIdAndStatusActive(string id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.TrelloId == id && u.Status == CommonEnums.USER_STATUS.ACTIVE);
            return user;
        }

        public async Task<User> GetUserByIdWithSkillList(int id)
        {
            User user = await _context.Users
                        .Where(u => u.Id == id && u.Status == CommonEnums.USER_STATUS.ACTIVE) 
                        .Include(u => u.UserSkills)
                        .ThenInclude(u => u.Skill)
                        .Include(u => u.Position)
                        .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetTrainerWithListTraineeByIdAndStatusActive(int id)
        {
            User user = await _context.Users
                        .Where(u => u.Id == id && u.Status == CommonEnums.USER_STATUS.ACTIVE)
                        .Include(u => u.Trainees)    
                        .FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetTraineeList(string keyword, int? positionId)
        {
            List<User> users = new();
            if (keyword == null && positionId == null)
            {
                users = await _context.Users
                .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE)
                .Include(u => u.Position)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            }
            else if (keyword == null)
            {
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE
                                && u.PositionId == positionId)
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            else if (positionId == null)
            {
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE
                                && (u.FirstName.ToLower().Contains(keyword) || u.LastName.ToLower().Contains(keyword) || u.Email.ToLower().Contains(keyword)))
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            else
            {
                users = await _context.Users
                .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE
                            && u.PositionId == positionId
                            && (u.FirstName.ToLower().Contains(keyword) || u.LastName.ToLower().Contains(keyword) || u.Email.ToLower().Contains(keyword)))
                .Include(u => u.Position)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            }
            return users;
        }

        public async Task<List<User>> GetTrainerList(string keyword, int? positionId)
        {
            List<User> users = new();
            if (keyword == null && positionId == null)
            {
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINER)
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            else if (keyword == null)
            {
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINER
                         && u.PositionId == positionId)
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            else if (positionId == null)
            {
                keyword = keyword.ToLower();
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINER
                         && (u.FirstName.ToLower().Contains(keyword) || u.LastName.ToLower().Contains(keyword) || u.Email.ToLower().Contains(keyword)))
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            else
            {
                keyword = keyword.ToLower();
                users = await _context.Users
                    .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINER
                          && u.PositionId == positionId
                          && (u.FirstName.ToLower().Contains(keyword) || u.LastName.ToLower().Contains(keyword) || u.Email.ToLower().Contains(keyword)))
                    .Include(u => u.Position)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
            }
            return users;
        }

        public async Task<List<User>> GetTraineeListByTrainerId(int id)
        {
            List<User> users = await _context.Users
                .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE && u.UserReferenceId==id)
                .Include(u => u.Position)
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> GetTraineeListByBatch(int batchid)
        {
            List<User> users = await _context.Users
                .Where(u => u.Status == CommonEnums.USER_STATUS.ACTIVE && u.Role == CommonEnums.ROLE.TRAINEE && u.OJTBatchId==batchid).OrderBy(c=>c.Id)
                .ToListAsync();
            return users;
        }
      
    }
}
