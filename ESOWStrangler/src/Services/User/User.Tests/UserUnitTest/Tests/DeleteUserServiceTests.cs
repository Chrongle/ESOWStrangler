using Xunit;
using NSubstitute;
using User.Core.Dtos;
using User.Core.Interfaces;
using User.Core.Services;
using Microsoft.Extensions.Logging;
public class DeleteUserServiceTests
{
  private readonly IRepository repositoryMock;
  private readonly IDeleteUserService service;
  private readonly ILogger<DeleteUserService> loggerMock;

  public DeleteUserServiceTests()
  {
    repositoryMock = Substitute.For<IRepository>();
    loggerMock = Substitute.For<ILogger<DeleteUserService>>();
    service = new DeleteUserService(repositoryMock, loggerMock);
  }

  [Fact]
  public async Task DeleteUserAsync_ShouldReturnUserId_WhenUserIsDeleted()
  {
    // Arrange
    var userId = 1;
    var request = new DeleteUserRequest { UserId = userId };
    var response = new DeleteUserResponse { UserId = userId };
    var cancellationToken = CancellationToken.None;

    repositoryMock.DeleteUserAsync(userId, cancellationToken).Returns(response);

    // Act
    var result = await service.DeleteUserAsync(request, cancellationToken);

    // Assert
    Assert.True(result.UserId == userId);
  }

  [Fact]
  public async Task DeleteUserAsync_ShouldNotReturnUserId_WhenUserIsNotDeleted()
  {
    // Arrange
    var userId = 1;
    var request = new DeleteUserRequest { UserId = userId };
    var cancellationToken = CancellationToken.None;

    repositoryMock.DeleteUserAsync(userId, cancellationToken).Returns((DeleteUserResponse)null);

    // Act
    var result = await service.DeleteUserAsync(request, cancellationToken);

    // Assert
    Assert.Null(result);
  }
}