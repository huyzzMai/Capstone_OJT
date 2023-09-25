using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetListNotificaitonByUser(int userId);

        Task<List<Notification>> GetListNotificaitonWithFilter(int userId, bool? status);

        Task<Notification> GetNotificationById(int id);  
    }
}
