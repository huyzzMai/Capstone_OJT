﻿using API.Hubs;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{

    [Route("api/[controller]")]
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
        [Route("GetCurrentUser")]
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
