using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Interfaces;

public interface IUserViewModelService
{
    Task<UserViewModel> GetRegisterUserViewModelAsync();
    Task UpdateUserAsync(UserViewModel userViewModel);
}
