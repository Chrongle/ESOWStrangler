using Microsoft.Extensions.Logging;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;

namespace User.Core.Services;

public class CreateUserService(IRepository repository, 
    ILogger<CreateUserService> logger) : ICreateUserService
{
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest userRequest, 
        CancellationToken cancellationToken)
    {
        var userEntity = new UserEntity
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Address = userRequest.Address,
            City = userRequest.City,
            ZipCode = userRequest.ZipCode,
            Country = userRequest.Country,
            Mobile = userRequest.Mobile,
            Email = userRequest.Email
        };

        logger.LogInformation("Creating user {@userEntity}", userEntity);
        var response = await repository.CreateUserAsync(userEntity, cancellationToken);

        return response;
    }
}