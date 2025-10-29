using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.Comercial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRCommerceApp.Core.Enums;

namespace HRCommerceApp.Web.Controllers
{
    [Authorize(Roles = UserRole.Administrador)]
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public IActionResult Ventas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarReporteVentas([FromBody] FiltroReporteDto filtro)
        {
            try
            {
                var reporte = await _reporteService.GenerarReporteVentasAsync(filtro.FechaInicio, filtro.FechaFin);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> ProductosMasVendidos()
        {
            try
            {
                var productos = await _reporteService.GetProductosMasVendidosAsync(10);
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar productos más vendidos: {ex.Message}";
                return View(new List<ProductoMasVendidoDto>());
            }
        }

        public async Task<IActionResult> VentasPorCliente()
        {
            try
            {
                var ventas = await _reporteService.GetVentasPorClienteAsync();
                return View(ventas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar ventas por cliente: {ex.Message}";
                return View(new List<VentaPorClienteDto>());
            }
        }
    }

    public class FiltroReporteDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}