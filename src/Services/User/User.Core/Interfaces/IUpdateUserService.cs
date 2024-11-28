using User.Core.Dtos;

namespace User.Core.Interfaces;
public interface IUpdateUserService
{
  Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest userRequest, CancellationToken cancellationToken);
}