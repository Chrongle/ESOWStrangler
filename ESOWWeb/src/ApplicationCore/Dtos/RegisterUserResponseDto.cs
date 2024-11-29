using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.Dtos;

public class RegisterUserResponseDto
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
