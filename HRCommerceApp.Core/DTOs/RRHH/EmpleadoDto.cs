using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.DTOs.RRHH
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal SalarioActual { get; set; }
        public DateTime FechaUltimoAumento { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Puesto { get; set; }
        public string Jerarquia { get; set; }
        public int DepartamentoId { get; set; }
        public string DepartamentoNombre { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public DateTime CreatedAt { get; set; }
    }

    public class CreateEmpleadoDto
    {
        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalarioActual { get; set; }

        [Required]
        [StringLength(100)]
        public string Puesto { get; set; }

        [Required]
        [StringLength(50)]
        public string Jerarquia { get; set; }

        [Required]
        public int DepartamentoId { get; set; }
    }

    public class UpdateEmpleadoDto
    {
        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalarioActual { get; set; }

        [Required]
        [StringLength(100)]
        public string Puesto { get; set; }

        [Required]
        [StringLength(50)]
        public string Jerarquia { get; set; }

        [Required]
        public int DepartamentoId { get; set; }
    }

    public class AumentoSalarialDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal NuevoSalario { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaAumento { get; set; }

        [StringLength(500)]
        public string Motivo { get; set; }
    }
}
