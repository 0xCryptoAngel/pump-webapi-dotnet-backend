using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PUMP_BACKEND.Services.Interfaces;

public class TokenService : ITokenService
{
    private readonly string _jwtSecret;
    private readonly int _jwtLifespanHours;

    public TokenService(IConfiguration config)
    {
        _jwtSecret = config["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret is missing.");
        _jwtLifespanHours = int.TryParse(config["Jwt:LifespanHours"], out var lifespan) ? lifespan : 1;
    }

    public string GenerateJwt(string username, string tenant)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("tenant", tenant)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtLifespanHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
