using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IUserService userService;

        public CommonController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpGet]
        [Authorize]
        [Route("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim != null)
            {              
                var user = await userService.GetUserById(int.Parse(userIdClaim.Value));             
                return Ok(user);
            }
            return Unauthorized();
        }
    }
}
