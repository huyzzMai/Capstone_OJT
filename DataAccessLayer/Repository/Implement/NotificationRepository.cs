using DataAccessLayer.Base;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Implement
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(OJTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<List<Notification>> GetListNotificaitonByUser(int userId)
        {
            List<Notification> res = await _context.Notifications
                                     .Where(u => u.UserId == userId)
                                     .OrderByDescending(u => u.CreatedAt)
                                     .ToListAsync();
            return res;
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            Notification u = await _context.Notifications
                .FirstOrDefaultAsync(u => u.Id == id);
            return u;
        }
    }
}
