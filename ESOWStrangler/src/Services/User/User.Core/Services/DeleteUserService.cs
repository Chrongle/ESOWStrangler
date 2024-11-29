using Microsoft.Extensions.Logging;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Core.Services;

public class DeleteUserService(IRepository repository, 
  ILogger<DeleteUserService> logger) : IDeleteUserService
{
  public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest userRequest, 
    CancellationToken cancellationToken)
  {
    logger.LogInformation("Deleting user with id {UserId}", userRequest.UserId);
    return await repository.DeleteUserAsync(userRequest.UserId, cancellationToken);
  }
}