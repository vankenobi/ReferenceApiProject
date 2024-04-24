## Reference API Project

This project is a reference API project designed with a clean architecture. It includes mappers, JWT token, PostgreSQL as the database, and SonarQube for code analysis. The Unit of Work design, repository pattern and FluentValidation are utilized.

### Folder Structure
    ├── Reference.Api
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
