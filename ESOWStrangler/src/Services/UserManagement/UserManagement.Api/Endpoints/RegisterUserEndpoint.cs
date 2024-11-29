using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FastEndpoints;
using UserManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Dtos;

namespace UserManagement.Api.Endpoints;
public class RegisterUserEndpoint(UserManager<User> userManager,
    IPublisherService rabbitMqService,
    ILogger<RegisterUserEndpoint> logger) : Endpoint<RegisterUserRequest, RegisterUserResponse>
{
    public override void Configure()
    {
        Post("/api/usermanagement/registeruser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            logger.LogError("Request is null");
            await SendAsync(new RegisterUserResponse
            {
                IsSuccessfulRegistration = false
            });
        }

        logger.LogInformation("Registering user with email {Email}", request.Email);

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await userManager.CreateAsync(user, request.Password);


        if (result.Succeeded)
        {
            logger.LogInformation("User with email {Email} has been registered", request.Email);
            await SendAsync(new RegisterUserResponse
            {
                IsSuccessfulRegistration = true
            });

            var crateUserMessageRequest = new CreateUserRequestMessagePublish
            {
              Email = request.Email,
              FirstName = request.FirstName,
              LastName = request.LastName,
              Address = request.Address,
              City = request.City,
              ZipCode = request.ZipCode,
              Country = request.Country,
              Mobile = request.Mobile
            };

            logger.LogInformation("Publishing user with email {Email} to RabbitMQ", request.Email);
            await rabbitMqService.Publish(crateUserMessageRequest,
              "user_registered");
        }
        else
        {
            logger.LogError("Failed to register user with email {Email}", request.Email);
            await SendAsync(new RegisterUserResponse
            {
                IsSuccessfulRegistration = false,
                Errors = result.Errors.Select(e => e.Description)
            });
        }
    }
}

public class RegisterUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string ZipCode { get; set; }
    public required string Country { get; set; }
    public required string Mobile { get; set; }
}

public class RegisterUserResponse
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
