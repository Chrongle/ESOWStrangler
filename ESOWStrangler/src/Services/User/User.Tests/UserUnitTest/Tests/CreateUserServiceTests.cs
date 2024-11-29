using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;
using User.Core.Services;
using Xunit;

public class CreateUserServiceTests
{
  private readonly IRepository repositoryMock;
  private readonly ICreateUserService service;
  private readonly ILogger<CreateUserService> loggerMock;

  public CreateUserServiceTests()
  {
    repositoryMock = Substitute.For<IRepository>();
    loggerMock = Substitute.For<ILogger<CreateUserService>>();
    service = new CreateUserService(repositoryMock, loggerMock);
  }

  [Fact]
  public async Task CreateUserAsync_ShouldReturnUser_WhenUserIsCreated()
  {
    // Arrange
    var request = GetCreateUserRequest();
    var userEntity = GetUserEntity();
    var response = GetCreateUserResponse(userEntity);
    var cancellationToken = CancellationToken.None;

    repositoryMock.CreateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns(response);

    // Act
    var result = await service.CreateUserAsync(request, cancellationToken);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userEntity.Id, result.UserId);
  }

  [Fact]
  public async Task CreateUserAsync_ShouldReturnNull_WhenUserIsNotCreated()
  {
    // Arrange
    var request = GetCreateUserRequest();
    var cancellationToken = CancellationToken.None;

    repositoryMock.CreateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns((CreateUserResponse)null);

    // Act
    var result = await service.CreateUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task CreateUserAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
  {
    // Arrange
    var request = GetCreateUserRequest();
    var cancellationToken = CancellationToken.None;

    repositoryMock.CreateUserAsync(Arg.Any<UserEntity>(), cancellationToken).Returns((CreateUserResponse)null);

    // Act
    var result = await service.CreateUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
  }

  private CreateUserRequest GetCreateUserRequest()
  {
    return new CreateUserRequest
    {
      FirstName = "John",
      LastName = "Doe",
      Address = "Vejlevej 16",
      City = "Vejle",
      ZipCode = "12345",
      Country = "Denmark",
      Mobile = "555-555-5555",
      Email = "john@mail.com"
    };
  }

  private UserEntity GetUserEntity()
  {
    return new UserEntity
    {
      Id = 1,
      FirstName = "John",
      LastName = "Doe",
      Address = "Vejlevej 16",
      City = "Vejle",
      ZipCode = "12345",
      Country = "Denmark",
      Mobile = "555-555-5555",
      Email = "john@mail.com"
    };
  }

  private CreateUserResponse GetCreateUserResponse(UserEntity userEntity)
  {
    return new CreateUserResponse
    {
      UserId = userEntity.Id,
    };
  }
}