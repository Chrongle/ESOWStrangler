using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;

namespace User.Core.Services;
public class UpdateUserService(IRepository repository,
  ILogger<UpdateUserService> logger) : IUpdateUserService
{
  public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest userRequest, 
    CancellationToken cancellationToken)
  {
    var userEntity = new UserEntity
    {
      Id = userRequest.UserId,
      FirstName = userRequest.FirstName,
      LastName = userRequest.LastName,
      Address = userRequest.Address,
      City = userRequest.City,
      ZipCode = userRequest.ZipCode,
      Country = userRequest.Country,
      Mobile = userRequest.Mobile,
      Email = userRequest.Email
    };

    logger.LogInformation("Updating user {@userEntity}", userEntity);
    return await repository.UpdateUserAsync(userEntity, cancellationToken);
  }
}