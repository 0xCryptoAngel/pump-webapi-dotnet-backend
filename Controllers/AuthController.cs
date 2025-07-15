using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PUMP_BACKEND.Entities;
using PUMP_BACKEND.Services;

namespace PUMP_BACKEND.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(
        ITokenService tokenService,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        
        if (request == null)
            return BadRequest("Missing or invalid request body.");

        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString();
        if (string.IsNullOrEmpty(tenant))
            return BadRequest("Tenant not resolved from subdomain.");

        var user = _userService.GetUserByUsername(tenant, request.Username);

        if (user == null || user.Password != request.Password)
            return Unauthorized();

        var token = _tokenService.GenerateJwt(user.Username, tenant);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString();
        var username = User.Identity?.Name;

        return Ok(new { Tenant = tenant, Username = username, Message = "Tenant-specific profile data." });
    }
}
