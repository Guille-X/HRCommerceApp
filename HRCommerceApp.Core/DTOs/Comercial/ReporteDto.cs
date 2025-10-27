using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.DTOs.Comercial
{
    public class ReporteVentasDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TotalVentas { get; set; }
        public int TotalTransacciones { get; set; }
        public decimal MargenGanancia { get; set; }
        public List<VentaPorClienteDto> VentasPorCliente { get; set; }
        public List<ProductoMasVendidoDto> ProductosMasVendidos { get; set; }
        public List<VentaDiariaDto> VentasDiarias { get; set; }
    }

    public class VentaPorClienteDto
    {
        public string ClienteNombre { get; set; }
        public decimal TotalVentas { get; set; }
        public int TotalCompras { get; set; }
    }

    public class ProductoMasVendidoDto
    {
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public int CantidadVendida { get; set; }
        public decimal TotalVentas { get; set; }
    }

    public class VentaDiariaDto
    {
        public DateTime Fecha { get; set; }
        public decimal TotalVentas { get; set; }
        public int TotalTransacciones { get; set; }
    }
}