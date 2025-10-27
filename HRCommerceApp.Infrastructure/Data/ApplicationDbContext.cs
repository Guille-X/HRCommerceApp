using HRCommerceApp.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HRCommerceApp.Core.Models;
using HRCommerceApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // RRHH
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<HistorialSalarial> HistorialSalarial { get; set; }

        // Comercial
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuraciones de modelo para RRHH
            builder.Entity<Empleado>(entity =>
            {
                entity.HasIndex(e => e.Documento).IsUnique();
                entity.HasOne(e => e.Departamento)
                      .WithMany(d => d.Empleados)
                      .HasForeignKey(e => e.DepartamentoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<HistorialSalarial>(entity =>
            {
                entity.HasOne(h => h.Empleado)
                      .WithMany(e => e.HistorialSalarial)
                      .HasForeignKey(h => h.EmpleadoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuraciones de modelo para Comercial
            builder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.SKU);
                entity.HasIndex(e => e.SKU).IsUnique();
            });

            builder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(e => e.Nit).IsUnique();
            });

            builder.Entity<Venta>(entity =>
            {
                entity.HasOne(v => v.Cliente)
                      .WithMany()
                      .HasForeignKey(v => v.IdCliente)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DetalleVenta>(entity =>
            {
                entity.HasOne(d => d.Venta)
                      .WithMany(v => v.Detalles)
                      .HasForeignKey(d => d.IdVenta)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                      .WithMany()
                      .HasForeignKey(d => d.SKU)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data inicial
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Datos iniciales para Departamentos
            builder.Entity<Departamento>().HasData(
                new Departamento
                {
                    IdDepartamento = 1,
                    Nombre = "Recursos Humanos",
                    Presupuesto = 50000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Departamento
                {
                    IdDepartamento = 2,
                    Nombre = "Ventas",
                    Presupuesto = 75000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Departamento
                {
                    IdDepartamento = 3,
                    Nombre = "TI",
                    Presupuesto = 60000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var auditable = (IAuditable)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    auditable.CreatedAt = DateTime.UtcNow;
                }

                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}