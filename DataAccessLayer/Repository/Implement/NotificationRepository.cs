using DataAccessLayer.Base;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
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

        public async Task<List<Notification>> GetListNotificaiton(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
