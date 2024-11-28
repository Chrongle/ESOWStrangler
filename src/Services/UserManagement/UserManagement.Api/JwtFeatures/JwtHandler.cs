using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Core.Entities;

namespace UserManagement.Api.JwtFeatures;

public class JwtHandler
{
    private readonly IConfiguration config;
    private readonly IConfigurationSection jwtSettings;
    private readonly ILogger<JwtHandler> logger;

    public JwtHandler(IConfiguration config, ILogger<JwtHandler> logger)
    {
        this.config = config;
        jwtSettings = config.GetSection("JWTSettings");
        this.logger = logger;
    }

    public string CreateToken(User user)
    {
        logger.LogInformation("Creating token for user {UserName}", user.UserName);
        try
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating token for user {UserName}", user.UserName);
            throw;
        }
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.UserName)
      };

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
        List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresInMinutes"])),
            signingCredentials: signingCredentials
            );

        return tokenOptions;
    }

}
