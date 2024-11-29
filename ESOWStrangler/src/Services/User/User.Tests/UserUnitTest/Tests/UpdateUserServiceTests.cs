using Xunit;
using NSubstitute;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;
using User.Core.Services;
using Microsoft.Extensions.Logging;

public class UpdateUserServiceTests
{
  private readonly IRepository repositoryMock;
  private readonly IUpdateUserService service;
  private readonly ILogger<UpdateUserService> loggerMock;

  public UpdateUserServiceTests()
  {
    repositoryMock = Substitute.For<IRepository>();
    loggerMock = Substitute.For<ILogger<UpdateUserService>>();
    service = new UpdateUserService(repositoryMock, loggerMock);
  }

  [Fact]
  public async Task UpdateUserAsync_ShouldReturnUser_WhenUserIsUpdated()
  {
    // Arrange
    var request = GetUpdateUserRequest();
    var userEntity = GetUserEntity();
    var response = GetUpdateUserResponse(userEntity);
    var cancellationToken = CancellationToken.None;

    repositoryMock.UpdateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns(response);

    // Act
    var result = await service.UpdateUserAsync(request, cancellationToken);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userEntity.Id, result.UserId);
  }

  [Fact]
  public async Task UpdateUserAsync_ShouldReturnNull_WhenUserIsNotUpdated()
  {
    // Arrange
    var request = GetUpdateUserRequest();
    var cancellationToken = CancellationToken.None;

    repositoryMock.UpdateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns((UpdateUserResponse)null);

    // Act
    var result = await service.UpdateUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task UpdateUserAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
  {
    // Arrange
    var request = GetUpdateUserRequest();
    var cancellationToken = CancellationToken.None;

    repositoryMock.UpdateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns((UpdateUserResponse)null);

    // Act
    var result = await service.UpdateUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
  }

  private UpdateUserRequest GetUpdateUserRequest()
  {
    return new UpdateUserRequest
    {
      UserId = 1,
      FirstName = "John",
      LastName = "Doe",
      Address = "123 Main St",
      City = "Anytown",
      ZipCode = "12345",
      Country = "USA",
      Mobile = "555-555-5555",
      Email = "john.doe@example.com"
    };
  }

  private UpdateUserResponse GetUpdateUserResponse(UserEntity userEntity)
  {
    return new UpdateUserResponse
    {
      UserId = userEntity.Id,
    };
  }
  private UserEntity GetUserEntity()
  {
    return new UserEntity
    {
      Id = 1,
      FirstName = "John",
      LastName = "Doe",
      Address = "123 Main St",
      City = "Anytown",
      ZipCode = "12345",
      Country = "USA",
      Mobile = "555-555-5555",
      Email = "john.doe@example.com"
    };
  }
}