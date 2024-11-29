using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Dtos;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class UserManagementService(HttpClient httpClient,
    ILogger<UserManagementService> logger) : IUserManagementService
{
    public async Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserDto userDto)
    {
        logger.LogInformation($"Creating user {userDto.Email}.");

        var response = await httpClient.PostAsJsonAsync("/gateway/api/usermanagement/registeruser", userDto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RegisterUserResponseDto>() ?? 
            throw new ArgumentNullException();
        if (result.IsSuccessfulRegistration)
        {
            logger.LogInformation($"User {userDto.Email} created.");
            return result;
        }
        
        logger.LogWarning($"User {userDto.Email} failed to create");
        return result;
    }

    public async Task<bool> SignInAsync(SignInUserRequest signInRequest)
    {
        logger.LogInformation($"Processing sign in request for user {signInRequest.Email}");

        var response = await httpClient.PostAsJsonAsync("/gateway/api/usermanagement/account/login", signInRequest);
        response.EnsureSuccessStatusCode();

        logger.LogInformation($"Successfully signed in user {signInRequest.Email}");
        return true;
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckEmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }
}
