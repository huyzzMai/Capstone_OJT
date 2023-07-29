using API.Hubs;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly IMemoryCache _cache;
        public CommonController(IUserService userService, IHubContext<SignalHub> hubContext, IMemoryCache cache)
        {
            this.userService = userService;
            _hubContext = hubContext;
            _cache = cache;
        }


        [HttpGet]
        [Authorize]
        [Route("current-user")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            var user = await userService.GetCurrentUserById(int.Parse(userIdClaim.Value));
            return Ok(user);
            //var cacheKey = "CurrentUser";
            //if (_cache.TryGetValue(cacheKey, out var cachedData))
            //{
            //    return Ok(cachedData);
            //}
            //else
            //{
            //    var user = await userService.GetCurrentUserById(int.Parse(userIdClaim.Value));
            //    _cache.Set(cacheKey, user, TimeSpan.FromMinutes(10));
            //    return Ok(user);
            //}
        }
    }
}
