using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Core.Services;
public class GetUserService(IRepository repository, 
  ILogger<GetUserService> logger) : IGetUserService
{
  public async Task<GetUserResponse> GetUserAsync(GetUserRequest userRequest,
    CancellationToken cancellationToken)
  {
    logger.LogInformation("Getting user with id {UserId}", userRequest.UserId);
    return await repository.GetUserAsync(userRequest.UserId, cancellationToken);
  }
}