using API.Hubs;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.Controllers.NotificationController
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        private readonly IHubContext<SignalHub> _hubContext;

        public NotificationController(INotificationService notificationService, IHubContext<SignalHub> hubContext)
        {
            this.notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateIsReadNotification(int notificationId)
        {
            try
            {

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
    }
}
