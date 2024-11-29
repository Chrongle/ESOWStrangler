namespace Microsoft.eShopWeb.ApplicationCore.Dtos;

public class RegisterUserDto
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
