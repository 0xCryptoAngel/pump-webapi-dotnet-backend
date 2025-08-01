using Microsoft.EntityFrameworkCore;
using PUMP_BACKEND.Models;
 
namespace PUMP_BACKEND.Data
    {
    public class PumpDbContext  : DbContext
    {
        public PumpDbContext(DbContextOptions<PumpDbContext> options) : base(options) {}
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Pump> Pumps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pump>(entity =>
                {
                    entity.HasKey(p => p.Id);
                    entity.Property(p => p.Id)
                        .ValueGeneratedOnAdd();
                    entity.HasOne(p => p.Tenant)
                        .WithMany(t => t.Pumps)
                        .HasForeignKey(p => p.TenantId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

        }

  }
}
