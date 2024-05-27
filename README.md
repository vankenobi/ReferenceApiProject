![example workflow](https://github.com/vankenobi/ReferenceApiProject/actions/workflows/build.yml/badge.svg)

## Reference API Project
This project is a reference API project designed with a clean architecture. It includes mappers, JWT token, PostgreSQL as the database, and SonarQube for code analysis. The Unit of Work design, repository pattern and FluentValidation are utilized.

## What technologies consist of this project ? 

This project was developed using technologies such as:

- [FluentValidation](https://fluentvalidation.net/)
- [JWT](https://jwt.io/) 
- AutoMapper
- UnitOfWork
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [CodeFirst](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database)
- [Serilog (SEQ)](https://serilog.net/) (With correlationId)
- [PostgreSQL](https://www.postgresql.org/)
- [Redis](https://redis.io/) (For cache)
- [Consul](https://www.consul.io/) (For service registration)
- [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware?view=aspnetcore-5.0)
  
### Folder Structure
    ├── Reference.Api
    │ ├── Cache
    │ │ ├── CacheService.cs
    │ │ └── ICacheService.cs
    │ ├── Controllers
    │ │ ├── AuthController.cs
    │ │ └── UserController.cs
    │ ├── Data
    │ │ └── DataContext.cs
    │ ├── Dtos
    │ │ ├── Requests
    │ │ │ ├── CreateUserRequest.cs
    │ │ │ ├── ICommonProperties.cs
    │ │ │ ├── LoginUserRequest.cs
    │ │ │ └── UpdateUserRequest.cs
    │ │ └── Responses
    │ │ └── GetUserResponse.cs
    │ ├── Extensions
    │ │ ├── ConfigureMappingProfileExtension.cs
    │ │ ├── HealthCheckConfigureExtension.cs
    │ │ ├── JwtBearerOptionsSetup.cs
    │ │ └── JwtOptionsSetup.cs
    │ ├── Mapper
    │ │ └── AutoMapperMappingProfile.cs
    │ ├── Models
    │ │ ├── BaseEntity.cs
    │ │ └── User.cs
    │ ├── Program.cs
    │ ├── Repositories
    │ │ ├── Implementations
    │ │ │ ├── GenericRepository.cs
    │ │ │ ├── UnitOfWork.cs
    │ │ │ └── UserRepository.cs
    │ │ └── Interfaces
    │ │ ├── IGenericRepository.cs
    │ │ ├── IUnitOfWork.cs
    │ │ └── IUserRepository.cs
    │ ├── Security
    │ │ ├── IJwtProvider.cs
    │ │ └── JwtOptions.cs
    │ ├── Services
    │ │ ├── Implementations
    │ │ │ ├── AuthService.cs
    │ │ │ └── UserService.cs
    │ │ └── Interfaces
    │ │ ├── IAuthService.cs
    │ │ └── IUserService.cs
    │ └── Utils
    │ ├── CreateUserValidator.cs
    │ ├── UpdateUserValidator.cs
    │ └── UserBaseValidator.cs
    └── Reference.Api.Test
    ├── AuthServiceFixture.cs
    ├── GlobalUsings.cs
    ├── JwtProviderFixture.cs
    ├── Reference.Api.Test.csproj
    ├── UpdateUserValidatorFixture.cs
    ├── UserBaseValidatorFixture.cs
    └── UserServiceFixture.cs
