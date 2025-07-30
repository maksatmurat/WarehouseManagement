using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<UnitOfMeasure> Units => Set<UnitOfMeasure>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Balance> Balances => Set<Balance>();
    public DbSet<ReceiptDocument> ReceiptDocuments => Set<ReceiptDocument>();
    public DbSet<ReceiptResource> ReceiptResources => Set<ReceiptResource>();
    public DbSet<ShipmentDocument> ShipmentDocuments => Set<ShipmentDocument>();
    public DbSet<ShipmentResource> ShipmentResources => Set<ShipmentResource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Автогенерация Guid для всех Id
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                idProperty.SetDefaultValueSql("NEWID()");
            }
        }

        // Установка IsActive = true по умолчанию
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isActiveProperty = entityType.FindProperty("IsActive");
            if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
            {
                isActiveProperty.SetDefaultValue(true);
            }
        }
        var adminId = Guid.NewGuid();
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminId,
                Fullname = "Super Admin",
                Email = "admin@example.com",
                Password = "admin123" 
            }
        );
        modelBuilder.Entity<SystemRole>().HasData(
           new SystemRole { Id = 1, Name = "Admin" },
           new SystemRole { Id = 2, Name = "User" }
        );
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = 1,
                UserId = adminId,
                RoleId = 1
            }
        );

        // Уникальные индексы
        modelBuilder.Entity<Resource>().HasIndex(r => r.Name).IsUnique();
        modelBuilder.Entity<UnitOfMeasure>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<ReceiptDocument>().HasIndex(d => d.Number).IsUnique();
        modelBuilder.Entity<ShipmentDocument>().HasIndex(d => d.Number).IsUnique();


        // Уникальная пара для Balance (ResourceId + UnitOfMeasureId)
        modelBuilder.Entity<Balance>()
            .HasIndex(b => new { b.ResourceId, b.UnitOfMeasureId })
            .IsUnique();

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.Resource)
            .WithMany(r => r.Balances)
            .HasForeignKey(b => b.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.UnitOfMeasure)
            .WithMany(u => u.Balances)
            .HasForeignKey(b => b.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptResource>()
            .HasOne(rr => rr.ReceiptDocument)
            .WithMany(rd => rd.Resources)
            .HasForeignKey(rr => rr.ReceiptDocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReceiptResource>()
            .HasOne(rr => rr.Resource)
            .WithMany(r => r.ReceiptResources)
            .HasForeignKey(rr => rr.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptResource>()
            .HasOne(rr => rr.UnitOfMeasure)
            .WithMany(u => u.ReceiptResources)
            .HasForeignKey(rr => rr.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentResource>()
            .HasOne(sr => sr.ShipmentDocument)
            .WithMany(sd => sd.ShipmentResources)
            .HasForeignKey(sr => sr.ShipmentDocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShipmentResource>()
            .HasOne(sr => sr.Resource)
            .WithMany(r => r.ShipmentResources)
            .HasForeignKey(sr => sr.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentResource>()
            .HasOne(sr => sr.UnitOfMeasure)
            .WithMany(u => u.ShipmentResources)
            .HasForeignKey(sr => sr.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentDocument>()
            .HasOne(sd => sd.Client)
            .WithMany(c => c.Shipments)
            .HasForeignKey(sd => sd.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}