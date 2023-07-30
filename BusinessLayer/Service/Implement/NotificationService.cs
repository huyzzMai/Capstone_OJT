using BusinessLayer.Models.ResponseModel.NotificationResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNotificaion(int userId, string title, string message, int type)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                var noti = new Notification()
                {
                    Title = title,
                    Message = message,  
                    Type = type,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddHours(7)
                };
                await _unitOfWork.NotificationRepository.Add(noti);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<NotificationResponse>> GetNotificationListForUser(int userId)
        {
            try
            {
                var notis = await _unitOfWork.NotificationRepository.GetListNotificaitonByUser(userId);
                List<NotificationResponse> res = notis.Select(
                    noti =>
                    {
                        return new NotificationResponse()
                        {
                            Title = noti.Title, 
                            Message = noti.Message,
                            Type = noti.Type ?? default(int),
                            IsRead = noti.IsRead ?? default(bool)
                        };
                    }
                    ).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task UpdateIsReadNotification(int userId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
