using FastEndpoints;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Api.Endpoints;

public class DeleteUserEndpoint(IDeleteUserService deleteUserService)
  : Endpoint<DeleteUserRequest, DeleteUserResponse>
{
  public override void Configure()
  {
    Delete("/api/user/{id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(DeleteUserRequest request, 
    CancellationToken cancellationToken)
  {
    var response = await deleteUserService.DeleteUserAsync(request, cancellationToken);
    await SendAsync(response);
  }
}