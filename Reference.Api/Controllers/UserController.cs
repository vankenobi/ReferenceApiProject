using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {

            // Bogus kütüphanesini kullanarak rastgele veri oluştur
            var faker = new Faker<CreateUserRequest>()
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Surname, f => f.Name.LastName())
                .RuleFor(u => u.Password, f => f.Internet.Password(8))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name.ToLower(), u.Surname.ToLower()));

            // 10 adet rastgele kullanıcı oluştur
            List<CreateUserRequest> users = faker.Generate(500);

            foreach (var user in users)
            {
                await _userService.CreateUser(user);
            }
            return StatusCode(201);

            //try
            //{
            //    var createdUserId = await _userService.CreateUser(createUserRequest);
            //    var userUri = Url.Action("GetUserById", new { id = createdUserId });

            //    return StatusCode(201, userUri);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An error occurred while creating the user");
            //    return StatusCode(500, "An error occurred while creating the user. Please try again later.");
            //}

        }

        [HttpDelete("{id}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.UpdateUser(updateUserRequest);
            if (!result)
                return NotFound($"Entity with id {updateUserRequest.Id} not found for updating");
            return NoContent();
        }

       
    }
}

