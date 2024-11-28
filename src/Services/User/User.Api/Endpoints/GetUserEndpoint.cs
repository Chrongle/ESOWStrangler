using FastEndpoints;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Api.Endpoints;

public class GetUserEndpoint(IGetUserService getUserService)
  : Endpoint<GetUserRequest, GetUserResponse>
{
  public override void Configure()
  {
    Get("/api/user/{id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetUserRequest request, 
    CancellationToken cancellationToken)
  {
    var response = await getUserService.GetUserAsync(request, cancellationToken);
    await SendAsync(response);
  }
}