using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUMP_BACKEND.Data;
using PUMP_BACKEND.Models;
using PUMP_BACKEND.Services.Interfaces;

namespace PUMP_BACKEND.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly PumpDbContext _pumpDb;

    public AuthController(
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        PumpDbContext pumpDb
        )
    {
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _pumpDb = pumpDb;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
            return BadRequest("Missing or invalid request body.");
        var subdomain = _httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString();
        var tenant = await _pumpDb.Tenants.Include(t => t.Users).FirstOrDefaultAsync(t => t.Name == subdomain);
        if (tenant == null)
            return BadRequest("Tenant not resolved from subdomain.");
        var user = tenant.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized("Invalid credentials");
        var token = _tokenService.GenerateJwt(user.Username, tenant.Name);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString();
        var username = User.Identity?.Name;

        return Ok(new { Tenant = tenant, Username = username});
    }
}
