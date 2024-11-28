using FastEndpoints;

namespace UserManagement.Api.Endpoints;
public class AuthTest() : Endpoint<AuthTestRequest, AuthTestResponse>
{
    public override void Configure()
    {
        Get("/api/authtest");
    }

    public override async Task HandleAsync(AuthTestRequest request, CancellationToken cancellationToken)
    {
        var response = new AuthTestResponse
        {
            IsAuthSuccessful = true,
        };
        await SendAsync(response);
    }
}

public class AuthTestRequest
{
  public bool Dummyprop { get; set; } = true;
}

public class AuthTestResponse
{
    public bool IsAuthSuccessful { get; set; }
}
