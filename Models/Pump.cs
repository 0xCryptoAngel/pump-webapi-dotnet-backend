namespace PUMP_BACKEND.Models;

public class Pump
{
    public int Id { get; set; }  // Primary Key
    public int TenantId { get; set; }  // Foreign Key

    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string AreaBlock { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int FlowRate { get; set; } // GPM
    public string Offset { get; set; } = default!;
    public int CurrentPressure { get; set; }
    public int MinPressure { get; set; }
    public int MaxPressure { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Navigation property
    public Tenant Tenant { get; set; } = default!;
}

