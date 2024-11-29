using User.Core.Dtos;

namespace User.Core.Interfaces;
public interface IDeleteUserService
{
  Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest userRequest, CancellationToken cancellationToken);
}