namespace PUMP_BACKEND.Models;

public class User
{
    public int UserId { get; set; }  // Primary Key
    public int TenantId { get; set; }  // Foreign Key

    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Navigation property
    public Tenant Tenant { get; set; } =  default!;
}



