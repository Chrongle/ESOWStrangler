using Microsoft.eShopWeb.ApplicationCore.Dtos;

namespace Microsoft.eShopWeb.Web.Interfaces;
public interface IUserManagementViewModelService
{
    Task RegisterUserAsync(RegisterUserDto registerUserDto);
}
