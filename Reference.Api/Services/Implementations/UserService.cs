using System;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Reference.Api.Cache;
using Reference.Api.Dtos.Requests;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Services.Interfaces;

namespace Reference.Api.Services.Implementations
{
	public class UserService : IUserService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IUserService> _logger;
        private readonly ICacheService _cacheService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IUserService> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<GetUserResponse> GetUserById(Guid id)
        {
            var cachedItem = await _cacheService.Get<User>(id.ToString());

            GetUserResponse? response;
            if (cachedItem != null)
            {
                response = _mapper.Map<GetUserResponse>(cachedItem);
                return response;
            }
            else
            {
                var user = await _unitOfWork.GetRepository<User>().GetById(id);
                if (user != null)
                    await _cacheService.Set(id.ToString(), user, DateTime.Now.AddMinutes(3));
                response = _mapper.Map<GetUserResponse>(user);
            }

            return response;
        }

        public async Task<Guid> CreateUser(CreateUserRequest createUserRequest)
        {
            var user = _mapper.Map<User>(createUserRequest);
            
            await _unitOfWork.GetRepository<User>().Add(user);
            await _unitOfWork.CompleteAsync();

            return user.Id;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.GetRepository<User>().GetById(id);
            bool hasCachedUser = await _cacheService.Exists(id.ToString());

            if (user == null)
            {
                _logger.LogWarning("Entity with id {Id} not found for deletion", id);
                return false; 
            }
            else
            {
                var result = _unitOfWork.GetRepository<User>().Delete(user);
                await _unitOfWork.CompleteAsync();

                if (hasCachedUser)
                    await _cacheService.Remove(id.ToString());

                return result;
            }
        }

        public async Task<bool> UpdateUser(UpdateUserRequest updateUserRequest)
        {
            var user = await _unitOfWork.GetRepository<User>().GetById(updateUserRequest.Id);
            bool hasCachedUser = await _cacheService.Exists(updateUserRequest.Id.ToString());

            if (user == null)
            {
                _logger.LogWarning("Entity with id {Id} not found for update", updateUserRequest.Id);
                return false;
            }
            else
            {
                var entity = _mapper.Map<User>(updateUserRequest);

                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedAt = user.CreatedAt;

                var result = _unitOfWork.GetRepository<User>().Upsert(entity);
                await _unitOfWork.CompleteAsync();

                if(hasCachedUser)
                    await _cacheService.Remove(updateUserRequest.Id.ToString());

                return result;
            }
        }
    }
}

