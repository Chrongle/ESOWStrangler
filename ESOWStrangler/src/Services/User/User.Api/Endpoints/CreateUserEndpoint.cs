using FastEndpoints;
using User.Core.Dtos;
using User.Core.Interfaces;

namespace User.Api.Endpoints;

public class CreateUserEndpoint(ICreateUserService createUserService)
  : Endpoint<CreateUserRequest, CreateUserResponse>
{
  public override void Configure()
  {
    Post("/api/user");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateUserRequest request, 
    CancellationToken cancellationToken)
  {
    var response = await createUserService.CreateUserAsync(request, cancellationToken);
    await SendAsync(response);
  }
}