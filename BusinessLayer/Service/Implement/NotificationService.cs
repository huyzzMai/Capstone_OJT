using BusinessLayer.Models.ResponseModel.NotificationResponse;
using BusinessLayer.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class NotificationService : INotificationService
    {
        public async Task CreateNotificaion(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NotificationResponse>> GetNotificationListForUser(int userId)
        {
            throw new NotImplementedException(); 
        }

        public async Task UpdateIsReadNotification(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
