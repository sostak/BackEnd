using Bakalauras.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Bakalauras.Core.Repositories.Interfaces;

namespace Bakalauras.Repository
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor for design-time creation
        public ApplicationDbContext()
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Mechanic> Mechanics { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Visit> Visits { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<InventoryOperation> InventoryOperations { get; set; } = null!;
        public DbSet<VisitType> VisitTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Load configuration from appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.UserId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Customer>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Mechanic>(entity =>
            {
                entity.ToTable("Mechanics");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.UserId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Mechanic>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.CustomerId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.Vehicles)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.ToTable("Visits");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.VehicleId).HasColumnType("varchar(255)");
                entity.Property(e => e.CustomerId).HasColumnType("varchar(255)");
                entity.Property(e => e.MechanicId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.Vehicle)
                    .WithMany(e => e.Visits)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.Visits)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Mechanic)
                    .WithMany(e => e.Visits)
                    .HasForeignKey(e => e.MechanicId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.MechanicId).HasColumnType("varchar(255)");
                entity.Property(e => e.VisitId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.Mechanic)
                    .WithMany(e => e.Services)
                    .HasForeignKey(e => e.MechanicId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Visit)
                    .WithMany(e => e.Services)
                    .HasForeignKey(e => e.VisitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.ToTable("InventoryItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.HasMany(e => e.InventoryOperations)
                    .WithOne(e => e.InventoryItem)
                    .HasForeignKey(e => e.InventoryItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InventoryOperation>(entity =>
            {
                entity.ToTable("InventoryOperations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(255)");
                entity.Property(e => e.InventoryItemId).HasColumnType("varchar(255)");
                entity.Property(e => e.ServiceId).HasColumnType("varchar(255)");
                entity.HasOne(e => e.Service)
                    .WithMany()
                    .HasForeignKey(e => e.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
