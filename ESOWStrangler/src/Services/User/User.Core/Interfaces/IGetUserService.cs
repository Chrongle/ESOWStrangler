using User.Core.Dtos;

namespace User.Core.Interfaces;
public interface IGetUserService
{
  Task<GetUserResponse> GetUserAsync(GetUserRequest userRequest, CancellationToken cancellationToken);
}