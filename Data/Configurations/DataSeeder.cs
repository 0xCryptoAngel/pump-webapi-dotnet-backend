using System;
using System.Linq;
using PUMP_BACKEND.Models;

namespace PUMP_BACKEND.Data.Configurations
{
    public class DataSeeder
    {
        private readonly PumpDbContext _context;

        public DataSeeder(PumpDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Tenants.Any())
            {
                // Seed Tenants
                var tenant1 = new Tenant
                {
                    TenantId = 1,
                    Name = "tenant1",
                    Email = "alpha@company.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("hashedpassword1"), // Replace with actual hash
                    CreatedAt = DateTime.UtcNow
                };

                var tenant2 = new Tenant
                {
                    TenantId = 2,
                    Name = "Tenant Beta",
                    Email = "beta@company.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("hashedpassword2"), // Replace with actual hash
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tenants.AddRange(tenant1, tenant2);
                _context.SaveChanges();

                // Seed Users
                var user1 = new User
                {
                    UserId = 1,
                    TenantId = tenant1.TenantId,
                    Username = "alpha_admin",
                    Email = "admin@alpha.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("hashedpassword1"), // Replace with actual hash
                    CreatedAt = DateTime.UtcNow
                };

                var user2 = new User
                {
                    UserId = 2,
                    TenantId = tenant1.TenantId,
                    Username = "alpha_user",
                    Email = "user@alpha.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("hashedpassword2"), // Replace with actual hash
                    CreatedAt = DateTime.UtcNow
                };

                var user3 = new User
                {
                    UserId = 3,
                    TenantId = tenant2.TenantId,
                    Username = "beta_admin",
                    Email = "admin@beta.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("hashedpassword3"), // Replace with actual hash
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.AddRange(user1, user2, user3);
                _context.SaveChanges();

                // Seed Pumps
                var pumps = new[]
                {
                    new Pump
                    {
                        Id = 1,
                        TenantId = tenant1.TenantId,
                        Name = "Pump A1",
                        Type = "Centrifugal",
                        AreaBlock = "Zone 1",
                        Latitude = 37.7749,
                        Longitude = -122.4194,
                        FlowRate = 100,
                        Offset = "0 sec",
                        CurrentPressure = 45,
                        MinPressure = 30,
                        MaxPressure = 60,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pump
                    {
                        Id = 2,
                        TenantId = tenant1.TenantId,
                        Name = "Pump A2",
                        Type = "Submersible",
                        AreaBlock = "Zone 2",
                        Latitude = 37.7750,
                        Longitude = -122.4195,
                        FlowRate = 120,
                        Offset = "1 sec",
                        CurrentPressure = 50,
                        MinPressure = 35,
                        MaxPressure = 65,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pump
                    {
                        Id = 3,
                        TenantId = tenant1.TenantId,
                        Name = "Pump A3",
                        Type = "Diaphragm",
                        AreaBlock = "Zone 3",
                        Latitude = 37.7751,
                        Longitude = -122.4196,
                        FlowRate = 90,
                        Offset = "2 sec",
                        CurrentPressure = 40,
                        MinPressure = 25,
                        MaxPressure = 55,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pump
                    {
                        Id = 4,
                        TenantId = tenant2.TenantId,
                        Name = "Pump B1",
                        Type = "Rotary",
                        AreaBlock = "Block B",
                        Latitude = 34.0522,
                        Longitude = -118.2437,
                        FlowRate = 110,
                        Offset = "3 sec",
                        CurrentPressure = 48,
                        MinPressure = 32,
                        MaxPressure = 66,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pump
                    {
                        Id = 5,
                        TenantId = tenant2.TenantId,
                        Name = "Pump B2",
                        Type = "Peristaltic",
                        AreaBlock = "Block C",
                        Latitude = 34.0523,
                        Longitude = -118.2438,
                        FlowRate = 130,
                        Offset = "4 sec",
                        CurrentPressure = 55,
                        MinPressure = 38,
                        MaxPressure = 72,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                _context.Pumps.AddRange(pumps);
                _context.SaveChanges();
            }
        }
    }
}
