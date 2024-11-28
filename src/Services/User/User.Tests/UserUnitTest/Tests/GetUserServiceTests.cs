using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using User.Core.Dtos;
using User.Core.Entities;
using User.Core.Interfaces;
using User.Core.Services;
using Xunit;

public class GetUserServiceTests
{
  private readonly IRepository repositoryMock;
  private readonly IGetUserService service;
  private readonly ILogger<GetUserService> loggerMock;

  public GetUserServiceTests()
  {
    repositoryMock = Substitute.For<IRepository>();
    loggerMock = Substitute.For<ILogger<GetUserService>>();
    service = new GetUserService(repositoryMock, loggerMock);
  }

  [Fact]
  public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var userId = 1;
    var userEntity = GetUserEntity();
    var request = new GetUserRequest { UserId = userId };
    var response = GetGetUserResponse(userEntity);
    var cancellationToken = CancellationToken.None;

    repositoryMock.GetUserAsync(userId, cancellationToken).Returns(response);

    // Act
    var result = await service.GetUserAsync(request, cancellationToken);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userEntity.Id, result.UserId);
    Assert.Equal(userEntity.FirstName, result.FirstName);
    Assert.Equal(userEntity.LastName, result.LastName);
    Assert.Equal(userEntity.Email, result.Email);
  }

  [Fact]
  public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
  {
    // Arrange
    var userId = 1;
    var request = new GetUserRequest { UserId = userId };
    var cancellationToken = CancellationToken.None;

    repositoryMock.GetUserAsync(userId, cancellationToken).Returns((GetUserResponse)null);

    // Act
    var result = await service.GetUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
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

  private GetUserResponse GetGetUserResponse(UserEntity userEntity)
  {
    return new GetUserResponse
    {
      UserId = userEntity.Id,
      FirstName = userEntity.FirstName,
      LastName = userEntity.LastName,
      Email = userEntity.Email,
      Address = userEntity.Address,
      City = userEntity.City,
      ZipCode = userEntity.ZipCode,
      Country = userEntity.Country,
      Mobile = userEntity.Mobile
    };
  }
}