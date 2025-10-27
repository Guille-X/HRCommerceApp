using HRCommerceApp.Core.DTOs.Comercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Interfaces
{
    public interface IReporteService
    {
        Task<ReporteVentasDto> GenerarReporteVentasAsync(DateTime? fechaInicio, DateTime? fechaFin);
        Task<IEnumerable<ProductoMasVendidoDto>> GetProductosMasVendidosAsync(int top = 10);
        Task<IEnumerable<VentaPorClienteDto>> GetVentasPorClienteAsync();
    }
}
