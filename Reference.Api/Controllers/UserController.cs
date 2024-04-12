using System;
using Microsoft.AspNetCore.Mvc;
using Reference.Api.Dtos.Requests;
using Reference.Api.Models;
using Reference.Api.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reference.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var item = await _userService.GetUserById(id);
                if (item == null)
                {
                    return NotFound("User not found");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user.");
                return StatusCode(500, "An error occurred, please try again later.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            try
            {
                var createdUserId = await _userService.CreateUser(createUserRequest);
                var userUri = Url.Action("GetUserById", new { id = createdUserId });

                return StatusCode(201, userUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user");
                return StatusCode(500, "An error occurred while creating the user. Please try again later.");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result)
                    return NotFound($"Entity with id {id} not found for deletion");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user");
                return StatusCode(500, "An error occurred while deleting the user. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.UpdateUser(updateUserRequest);
            if (!result)
                return NotFound($"Entity with id {updateUserRequest.Id} not found for updating");
            return NoContent();
        }

       
    }
}

