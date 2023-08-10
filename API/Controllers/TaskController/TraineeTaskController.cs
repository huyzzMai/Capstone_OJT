using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Models.RequestModel;
using BusinessLayer.Utilities;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using DataAccessLayer.Commons;
using Microsoft.IdentityModel.Protocols;
using System.IO;
using System.Net;
using TrelloDotNet.Model.Webhook;
using TrelloDotNet.Model;
using TrelloDotNet;
using Microsoft.Extensions.Configuration;
using System.Drawing.Text;

namespace API.Controllers.TaskController
{
    [Route("api/trainee-tasks")]
    [ApiController]
    [Authorize(Roles = "Trainee")]
    public class TraineeTaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;
        public IConfiguration _configuration;

        public TraineeTaskController(ITaskService taskService, IUserService userService, IHubContext<SignalHub> hubContext, IConfiguration configuration)
        {
            this.taskService = taskService;
            this.userService = userService;
            _hubContext = hubContext;
            _configuration = configuration; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskOfTrainee([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetAllTaskOfTrainee(userId, paging));
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

        [HttpGet("accomplished-tasks")]
        public async Task<IActionResult> GetListFinishTaskOfTrainee([FromQuery] PagingRequestModel paging)
        {
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                // Get id of current log in user 
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                return Ok(await taskService.GetListTaskAccomplished(userId, paging));
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

        //[HttpPost("{taskId}")]
        //public async Task<IActionResult> UpdateFinishTask(string taskId)
        //{
        //    try
        //    {
        //        int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
        //        await taskService.CreateFinishTask(userId, taskId);
        //        await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_FINISH);
        //        return StatusCode(StatusCodes.Status201Created, "Accomplish task successfully.");
        //    }
        //    catch (ApiException e)
        //    {
        //        return StatusCode(e.StatusCode, e.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message);
        //    }
        //}

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhookOnDueTaskComplete(HttpRequestData req)
        {
            //var trelloClientHelper = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
            try
            {
                //Get The raw JSON from the webhook and process it
                byte[] content = req.Body;
                Stream stream = new MemoryStream(content);
                using var streamReader = new StreamReader(stream);
                var json = await streamReader.ReadToEndAsync(); //JSON from a Board Webhook

                //Get a configured Trello client
                var trelloClient = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);

                var webhookDataReceiver = new TrelloDotNet.WebhookDataReceiver(trelloClient);

                //you can subscribe to more curated events (Few but common events people need)
                webhookDataReceiver.SmartEvents.OnDueCardIsMarkedAsComplete += OnDueCardIsMarkedAsComplete;

                webhookDataReceiver.ProcessJsonIntoEvents(json);    
                return Ok("Update task successfully!");
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

        private async void OnDueCardIsMarkedAsComplete(WebhookSmartEventDueMarkedAsComplete args)
        {
            var check = args.CardId;
            await taskService.CreateFinishTask(args.CardId);
            await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_FINISH);
        }
    }
}
