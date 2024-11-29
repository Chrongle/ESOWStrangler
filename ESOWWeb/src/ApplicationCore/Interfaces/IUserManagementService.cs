using Microsoft.eShopWeb.ApplicationCore.Dtos;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IUserManagementService
{
    Task<RegisterUserResponseDto> CreateUserAsync(RegisterUserDto userDto);
    Task<bool> SignInAsync(SignInUserRequest signInRequest);
    Task SignOutAsync();
    Task<bool> CheckEmailExistsAsync(string email);
}
