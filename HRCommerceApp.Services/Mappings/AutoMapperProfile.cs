using AutoMapper;
using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Services.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // RRHH Mappings
            CreateMap<Departamento, DepartamentoDto>();
            CreateMap<CreateDepartamentoDto, Departamento>();
            CreateMap<UpdateDepartamentoDto, Departamento>();

            CreateMap<Empleado, EmpleadoDto>()
                .ForMember(dest => dest.DepartamentoNombre, opt => opt.MapFrom(src => src.Departamento.Nombre));
            CreateMap<CreateEmpleadoDto, Empleado>();
            CreateMap<UpdateEmpleadoDto, Empleado>();

            CreateMap<HistorialSalarial, HistorialSalarialDto>();

            // Comercial Mappings
            CreateMap<Producto, ProductoDto>();
            CreateMap<Cliente, ClienteDto>();
            CreateMap<Venta, VentaDto>();
            CreateMap<DetalleVenta, DetalleVentaDto>();
        }
    }

    // DTOs adicionales para Comercial
    public class ProductoDto
    {
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public int Existencia { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
    }

    public class VentaDto
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
    }

    public class DetalleVentaDto
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public string SKU { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public decimal Total { get; set; }
    }

    public class HistorialSalarialDto
    {
        public int IdHistorial { get; set; }
        public int EmpleadoId { get; set; }
        public decimal SalarioAnterior { get; set; }
        public decimal NuevoSalario { get; set; }
        public decimal Aumento { get; set; }
        public decimal PorcentajeAumento { get; set; }
        public DateTime FechaAumento { get; set; }
        public string Motivo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}