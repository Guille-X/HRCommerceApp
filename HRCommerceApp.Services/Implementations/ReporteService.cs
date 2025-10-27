using HRCommerceApp.Core.DTOs.Comercial;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Services.Implementations
{
    public class ReporteService : IReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReporteVentasDto> GenerarReporteVentasAsync(DateTime? fechaInicio, DateTime? fechaFin)
        {
            fechaInicio ??= DateTime.UtcNow.AddMonths(-1);
            fechaFin ??= DateTime.UtcNow;

            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                .ToListAsync();

            var totalVentas = ventas.Sum(v => v.Monto);
            var totalTransacciones = ventas.Count;

            var costoTotal = ventas.Sum(v => v.Detalles.Sum(d => d.Costo * d.Cantidad));
            var margenGanancia = totalVentas - costoTotal;

            var ventasPorCliente = ventas
                .GroupBy(v => v.Cliente.Nombre)
                .Select(g => new VentaPorClienteDto
                {
                    ClienteNombre = g.Key,
                    TotalVentas = g.Sum(v => v.Monto),
                    TotalCompras = g.Count()
                })
                .ToList();

            var productosMasVendidos = ventas
                .SelectMany(v => v.Detalles)
                .GroupBy(d => new { d.SKU, d.Producto.Descripcion })
                .Select(g => new ProductoMasVendidoDto
                {
                    SKU = g.Key.SKU,
                    Descripcion = g.Key.Descripcion,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVentas = g.Sum(d => d.Total)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .Take(10)
                .ToList();

            var ventasDiarias = ventas
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new VentaDiariaDto
                {
                    Fecha = g.Key,
                    TotalVentas = g.Sum(v => v.Monto),
                    TotalTransacciones = g.Count()
                })
                .OrderBy(v => v.Fecha)
                .ToList();

            return new ReporteVentasDto
            {
                FechaInicio = fechaInicio.Value,
                FechaFin = fechaFin.Value,
                TotalVentas = totalVentas,
                TotalTransacciones = totalTransacciones,
                MargenGanancia = margenGanancia,
                VentasPorCliente = ventasPorCliente,
                ProductosMasVendidos = productosMasVendidos,
                VentasDiarias = ventasDiarias
            };
        }

        public async Task<IEnumerable<ProductoMasVendidoDto>> GetProductosMasVendidosAsync(int top = 10)
        {
            var productos = await _context.DetalleVentas
                .Include(d => d.Producto)
                .GroupBy(d => new { d.SKU, d.Producto.Descripcion })
                .Select(g => new ProductoMasVendidoDto
                {
                    SKU = g.Key.SKU,
                    Descripcion = g.Key.Descripcion,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVentas = g.Sum(d => d.Total)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .Take(top)
                .ToListAsync();

            return productos;
        }

        public async Task<IEnumerable<VentaPorClienteDto>> GetVentasPorClienteAsync()
        {
            var ventasPorCliente = await _context.Ventas
                .Include(v => v.Cliente)
                .GroupBy(v => new { v.IdCliente, v.Cliente.Nombre })
                .Select(g => new VentaPorClienteDto
                {
                    ClienteNombre = g.Key.Nombre,
                    TotalVentas = g.Sum(v => v.Monto),
                    TotalCompras = g.Count()
                })
                .OrderByDescending(v => v.TotalVentas)
                .ToListAsync();

            return ventasPorCliente;
        }
    }
}
