using Microsoft.EntityFrameworkCore;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;
using User.Infrastructure.EFCore.Context;

namespace User.Infrastructure.Repositories;

public class Repository(UserDbContext userDbContext) : IRepository
{
  public async Task<CreateUserResponse> CreateUserAsync(UserEntity userEntity,
    CancellationToken cancellationToken)
  {
    userDbContext.Users.Add(userEntity);
    await userDbContext.SaveChangesAsync(cancellationToken);
    return new CreateUserResponse
    {
      UserId = userEntity.Id
    };
  }

  public async Task<DeleteUserResponse> DeleteUserAsync(int userId,
    CancellationToken cancellationToken)
  {
    var userEntity = await userDbContext.Users.FindAsync(userId, cancellationToken) ??
      throw new ArgumentNullException("User not found");
    userDbContext.Users.Remove(userEntity);
    await userDbContext.SaveChangesAsync(cancellationToken);
    return new DeleteUserResponse
    {
      UserId = userId
    };
  }

  public async Task<GetUserResponse> GetUserAsync(int userId,
    CancellationToken cancellationToken)
  {
    var userEntity = await userDbContext.Users.FindAsync(userId, cancellationToken) ??
      throw new ArgumentNullException("User not found");
    return new GetUserResponse
    {
      UserId = userEntity.Id,
      FirstName = userEntity.FirstName,
      LastName = userEntity.LastName,
      Address = userEntity.Address,
      City = userEntity.City,
      ZipCode = userEntity.ZipCode,
      Country = userEntity.Country,
      Mobile = userEntity.Mobile,
      Email = userEntity.Email
    };
  }

  public async Task<UpdateUserResponse> UpdateUserAsync(UserEntity userEntity,
    CancellationToken cancellationToken)
  {
    userDbContext.Users.Update(userEntity);
    return await userDbContext.SaveChangesAsync(cancellationToken) > 0
      ? new UpdateUserResponse
      {
        UserId = userEntity.Id
      }
      : throw new Exception("Failed to update user");
  }
}