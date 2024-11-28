using User.Core.Entities;
using User.Core.Dtos;
namespace User.Core.Interfaces;
public interface IRepository
{
  Task<CreateUserResponse> CreateUserAsync(UserEntity userEntity, CancellationToken cancellationToken);
  Task<GetUserResponse> GetUserAsync(int userId, CancellationToken cancellationToken);
  Task<DeleteUserResponse> DeleteUserAsync(int userId, CancellationToken cancellationToken);
  Task<UpdateUserResponse> UpdateUserAsync(UserEntity userEntity, CancellationToken cancellationToken);
}