using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Services.Implementations;
using Reference.Api.Services.Interfaces;
using Reference.Api.Dtos.Requests;
using Reference.Api.Cache;

namespace Reference.Api.Test;

[TestFixture]
public class UserServiceFixture 
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<IUserService>> _mockLogger;
    private UserService _userService;
    private Mock<ICacheService> _cacheService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<IUserService>>();
        _cacheService = new Mock<ICacheService>();
        _userService = new UserService(_mockUnitOfWork.Object,_mockMapper.Object,_mockLogger.Object,_cacheService.Object);
    }

    [Test]
    public async Task GetUserById_ReturnsUser()
    {
        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        GetUserResponse getUserResponseMockData = new()
        {
            FullName = "John Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(userMockData.Id)).ReturnsAsync(userMockData);
        _mockMapper.Setup(x => x.Map<GetUserResponse>(userMockData)).Returns(getUserResponseMockData);

        var result = await _userService.GetUserById(userMockData.Id);

        #endregion
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(userMockData.Email));
    }

    [Test]
    public async Task GetUserById_WhenCachedItemNotNull_ReturnsUser()
    {
        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        GetUserResponse getUserResponseMockData = new()
        {
            FullName = "John Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(userMockData.Id)).ReturnsAsync(userMockData);
        _mockMapper.Setup(x => x.Map<GetUserResponse>(userMockData)).Returns(getUserResponseMockData);
        _cacheService.Setup(x => x.Get<User>(userMockData.Id.ToString())).ReturnsAsync(userMockData);

        var result = await _userService.GetUserById(userMockData.Id);

        #endregion

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(userMockData.Email));
    }

    [Test]
    public async Task CreateUser_ReturnsCreatedUserId()
    {
        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        CreateUserRequest createUserRequestMockData = new()
        {
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword"
        };

        #endregion

        #region Mocking

        _mockMapper.Setup(x => x.Map<User>(createUserRequestMockData)).Returns(userMockData);
        _mockUnitOfWork.Setup(x => x.GetRepository<User>().Add(userMockData)).ReturnsAsync(true);

        _mockMapper.Setup(x => x.Map<User>(createUserRequestMockData)).Returns(userMockData);

        var result = await _userService.CreateUser(createUserRequestMockData);

        #endregion

        Assert.That(result, Is.Not.Empty);
        Assert.That(userMockData.Id, Is.EqualTo(result));
    }

    [Test]
    public async Task DeleteUser_WhenUserNotFound_ReturnsFalse()
    {

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(Guid.NewGuid())).ReturnsAsync((User)null);

        var result = await _userService.DeleteUser(Guid.NewGuid());

        #endregion

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteUser_WhenUserDeleteSuccess_ReturnsTrue()
    {

        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(userMockData.Id)).ReturnsAsync(userMockData);
        _mockUnitOfWork.Setup(x => x.GetRepository<User>().Delete(userMockData)).Returns(true);

        var result = await _userService.DeleteUser(userMockData.Id);

        #endregion

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteUser_WhenUserDeleteSuccessAndHasCachedUserIsTrue_ReturnsTrue()
    {

        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(userMockData.Id)).ReturnsAsync(userMockData);
        _mockUnitOfWork.Setup(x => x.GetRepository<User>().Delete(userMockData)).Returns(true);
        _cacheService.Setup(x => x.Exists(userMockData.Id.ToString())).ReturnsAsync(true);

        var result = await _userService.DeleteUser(userMockData.Id);

        #endregion

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UpdateUser_WhenUserNotFound_ReturnsFalse()
    {

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(Guid.NewGuid())).ReturnsAsync((User)null);

        var result = await _userService.DeleteUser(Guid.NewGuid());

        #endregion

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateUser_WhenUserUpdateSuccess_ReturnsTrue()
    {
        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        UpdateUserRequest updateUserRequestMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword"
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(updateUserRequestMockData.Id)).ReturnsAsync(userMockData);
        _mockUnitOfWork.Setup(x => x.GetRepository<User>().Upsert(userMockData)).Returns(true);
        _mockMapper.Setup(x => x.Map<User>(updateUserRequestMockData)).Returns(userMockData);

        var result = await _userService.UpdateUser(updateUserRequestMockData);

        #endregion

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UpdateUser_WhenUserUpdateSuccessAndhasCachedUserIsTrue_ReturnsTrue()
    {
        #region MockData

        User userMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        UpdateUserRequest updateUserRequestMockData = new()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Email = "john@gmail.com",
            Password = "p@ssword"
        };

        #endregion

        #region Mocking

        _mockUnitOfWork.Setup(x => x.GetRepository<User>().GetById(updateUserRequestMockData.Id)).ReturnsAsync(userMockData);
        _mockUnitOfWork.Setup(x => x.GetRepository<User>().Upsert(userMockData)).Returns(true);
        _mockMapper.Setup(x => x.Map<User>(updateUserRequestMockData)).Returns(userMockData);
        _cacheService.Setup(x => x.Exists(updateUserRequestMockData.Id.ToString())).ReturnsAsync(true);

        var result = await _userService.UpdateUser(updateUserRequestMockData);

        #endregion

        Assert.That(result, Is.True);
    }

}
