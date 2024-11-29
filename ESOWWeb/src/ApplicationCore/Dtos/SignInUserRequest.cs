namespace Microsoft.eShopWeb.ApplicationCore.Dtos;

public class SignInUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
