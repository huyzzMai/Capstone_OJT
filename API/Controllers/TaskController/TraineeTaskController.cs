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

        [HttpPost("{taskId}")]
        public async Task<IActionResult> UpdateFinishTask(string taskId)
        {
            try
            {
                int userId = userService.GetCurrentLoginUserId(Request.Headers["Authorization"]);
                await taskService.CreateFinishTask(userId, taskId);
                await _hubContext.Clients.All.SendAsync(CommonEnumsMessage.TASK_MESSAGE.UPDATE_FINISH);
                return StatusCode(StatusCodes.Status201Created, "Accomplish task successfully.");
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

        [HttpPost("webhook")]
        public async Task<IActionResult> Run(HttpRequestData req)
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

                //Option 1: Use code to parse the event yourself
                WebhookNotificationBoard webhookBoard = webhookDataReceiver.ConvertJsonToWebhookNotificationBoard(json); //Alternative you can get data from a list of a card if that is what you subscribed to in your Webhook

                Board board = webhookBoard.Board; //Info on the source-board
                WebhookAction action = webhookBoard.Action; //The Webhook-action (generic if webhook is Board, List or Card)
                string actionType = action.Type; //The type of event (Example: 'UpdateCard' or 'AddLabelToCard')
                WebhookActionData webhookActionData = action.Data; //Data about the event (aka what board, list, card, etc. was involved)
                WebhookActionDisplay webhookActionDisplay = action.Display; //Display name of the Action (can sometime help better understand the event)
                Member actionMemberCreator = action.MemberCreator; //The member who did the action

                //Todo: Use above to react and do what you need to do

                //--------------------------------------------------------------------------------------------------------------
                //Option 2: Let the Webhook Receiver send you C# Event based on the incoming data

                //You can subscribe to basic raw events (there are over 70 of these)
                webhookDataReceiver.BasicEvents.OnUpdateCard += BasicEvents_OnUpdateCard;
                webhookDataReceiver.BasicEvents.OnAddLabelToCard += BasicEvents_OnAddLabelToCard;

                //Or you can subscribe to more curated events (Few but common events people need)
                webhookDataReceiver.SmartEvents.OnCardMovedToNewList += SmartEvents_OnCardMovedToNewList;
                webhookDataReceiver.SmartEvents.OnLabelAddedToCard += SmartEvents_OnLabelAddedToCard;

            }
            catch (Exception e)
            {
                //todo - deal with exceptions (error mail or the like) and potential retry
            }

            return req.CreateResponse(HttpStatusCode.OK);//This is still OK as Trello should not see exception as the receiver 
        }
    }
}
