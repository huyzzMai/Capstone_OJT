using API.Hubs;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.Controllers.NotificationController
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public NotificationController(IUserService userService, INotificationService notificationService, IHubContext<SignalHub> hubContext)
        {
            this.notificationService = notificationService;
            this.userService = userService;
            _hubContext = hubContext;
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateIsReadNotification(int notificationId)
        {
            try
            {
                await notificationService.UpdateIsReadNotification(notificationId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.UPDATE_NOTI);
                return Ok("Notification is read");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIsReadNotificationList()
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await notificationService.UpdateIsReadNotificationList(userId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.UPDATE_NOTI);
                return Ok("Notification read");
            }
            catch (ApiException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationList([FromQuery] int? statusRead)
        {
            try
            {
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await notificationService.GetNotificationListForUser(userId, statusRead)); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("batch/{batchId}")]
        public async Task<IActionResult> CreateBatchNotification(int batchId)
        {
            try
            {
                await notificationService.CreateBatchNotificationForTrainer(batchId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.NOTIFICATION_MESSAGE.CREATE_NOTI);
                return Ok("Create notification successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
