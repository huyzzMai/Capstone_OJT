using BusinessLayer.Payload.ResponseModel.NotificationResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Spreadsheet;
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
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UserId = userId
                };
                await _unitOfWork.NotificationRepository.Add(noti);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateBatchNotificationForTrainer(int batchId)
        {
            try
            {
                var trainees = await _unitOfWork.UserRepository.GetTraineeListByBatch(batchId);
                bool hasNoTrainer = trainees.Any(trainee => trainee.UserReferenceId == null);
                if (hasNoTrainer)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Some trainees have not been assigned to any trainers!");
                }
                var filteredList = trainees
                                   .GroupBy(x => x.UserReferenceId)
                                   .Select(group => group.First()) 
                                   .ToList();
                var batch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.Id == batchId);
                foreach (var trainee in filteredList)
                {
                    var noti = new Notification()
                    {
                        Title = "Yêu cầu đánh giá khóa thực tập",
                        Message = "khóa thực tập "+ batch.Name + " cần được đánh giá gấp.",
                        Type = CommonEnums.NOTIFICATION_TYPE.BATCH_TYPE,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        UserId = trainee.UserReferenceId ?? default
                    };
                    await _unitOfWork.NotificationRepository.Add(noti);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<NotificationResponse>> GetNotificationListForUser(int userId, bool? status)
        {
            try
            {
                var notis = await _unitOfWork.NotificationRepository.GetListNotificaitonWithFilter(userId, status);
                List<NotificationResponse> res = notis.Select(
                    noti =>
                    {
                        return new NotificationResponse()
                        {
                            Id = noti.Id,
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

        public async Task UpdateIsReadNotification(int notiId)
        {
            try
            {
                var noti = await _unitOfWork.NotificationRepository.GetNotificationById(notiId);
                if (noti == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "This notification not found!");
                }
                noti.IsRead = true; 
                await _unitOfWork.NotificationRepository.Update(noti);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateIsReadNotificationList(int userId)
        {
            try
            {
                var notis = await _unitOfWork.NotificationRepository.GetListNotificaitonByUser(userId);
                if (notis.Count != 0)
                {
                    foreach (var noti in notis)
                    {
                        if (noti.IsRead != true)
                        {
                            noti.IsRead = true;
                            await _unitOfWork.NotificationRepository.Update(noti);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
