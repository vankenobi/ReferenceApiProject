using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reference.Api.Dtos.Requests;
using Reference.Api.Services.Interfaces;

namespace Reference.Api.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
        {
            var token = await _authService.Login(loginUserRequest);
            return Ok(token);
        }
    }
}

