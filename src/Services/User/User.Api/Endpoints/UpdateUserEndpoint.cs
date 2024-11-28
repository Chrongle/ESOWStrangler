using FastEndpoints;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Api.Endpoints;

public class UpdateUserEndpoint(IUpdateUserService updateUserService)
  : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
  public override void Configure()
  {
    Put("/api/user/{id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdateUserRequest request, 
    CancellationToken cancellationToken)
  {
    var response = await updateUserService.UpdateUserAsync(request, cancellationToken);
    await SendAsync(response);
  }
}