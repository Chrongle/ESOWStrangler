using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using UserManagement.Api.JwtFeatures;
using UserManagement.Core.Entities;

namespace UserManagement.Api.Endpoints;
public class AuthenticateUser(JwtHandler jwtHandler,
    UserManager<User> userManager,
    ILogger<AuthenticateUser> logger) : Endpoint<AuthenticateUserRequest, AuthenticateUserResponse>
{
    public override void Configure()
    {
        Post("/api/usermanagement/auth");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogWarning("Invalid email or password for user {Email}", request.Email);
            await SendUnauthorizedAsync();
        }

        var token = jwtHandler.CreateToken(user);

        var response = new AuthenticateUserResponse
        {
            IsAuthSuccessful = true,
            Token = token
        };

        logger.LogInformation("User {Email} has been authenticated", request.Email);
        await SendAsync(response);
    }

}

public class AuthenticateUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class AuthenticateUserResponse
{
    public bool IsAuthSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}
