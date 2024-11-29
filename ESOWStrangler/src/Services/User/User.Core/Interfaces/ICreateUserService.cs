using User.Core.Dtos;

namespace User.Core.Interfaces;
public interface ICreateUserService
{
  Task<CreateUserResponse> CreateUserAsync(CreateUserRequest userRequest, CancellationToken cancellationToken);
}