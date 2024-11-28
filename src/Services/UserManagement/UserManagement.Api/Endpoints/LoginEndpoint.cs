using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using UserManagement.Core.Entities;

namespace UserManagement.Api.Endpoints;

public class LoginEndpoint(UserManager<User> userManager,
  SignInManager<User> signInManager,
  ILogger<LoginEndpoint> logger) : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/api/usermanagement/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            logger.LogWarning("Invalid email or password for user {Email}", request.Email);
            await SendAsync(new LoginResponse
            {
                IsSuccessfulLogin = false,
                Errors = new[] { "Invalid login attempt." }
            });
            return;
        }

        logger.LogInformation("Logging in user with email {Email}", request.Email);
        var result = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (result.Succeeded)
        {
            logger.LogInformation("User with email {Email} has been logged in", request.Email);
            await SendAsync(new LoginResponse
            {
                IsSuccessfulLogin = true
            });
        }
        else
        {
            logger.LogWarning("Invalid email or password for user {Email}", request.Email);
            await SendAsync(new LoginResponse
            {
                IsSuccessfulLogin = false,
                Errors = new[] { "Invalid login attempt." }
            });
        }
    }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponse
{
    public bool IsSuccessfulLogin { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
