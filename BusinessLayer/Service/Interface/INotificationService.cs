using BusinessLayer.Payload.ResponseModel.NotificationResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface INotificationService
    {
        Task CreateNotificaion(int userId, string title, string message, int type);
        Task CreateBatchNotificationForTrainer(int batchId);
        Task<List<NotificationResponse>> GetNotificationListForUser(int userId, int? status);  
        Task UpdateIsReadNotification(int notiId);
        Task UpdateIsReadNotificationList(int userId);
    }
}
