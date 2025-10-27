using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.Comercial;
using HRCommerceApp.Core.Enums;

namespace HRCommerceApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("ventas")]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> GetReporteVentas([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var reporte = await _reporteService.GenerarReporteVentasAsync(fechaInicio, fechaFin);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("productos-mas-vendidos")]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> GetProductosMasVendidos([FromQuery] int top = 10)
        {
            try
            {
                var productos = await _reporteService.GetProductosMasVendidosAsync(top);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ventas-por-cliente")]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> GetVentasPorCliente()
        {
            try
            {
                var ventas = await _reporteService.GetVentasPorClienteAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}