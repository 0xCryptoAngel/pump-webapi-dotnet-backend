using PUMP_BACKEND.Models;

public class Tenant
{
    public int TenantId { get; set; }  // Primary Key
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Pump> Pumps { get; set; } = new List<Pump>();
}
