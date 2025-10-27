using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.DTOs.RRHH
{
    public class DepartamentoDto
    {
        public int IdDepartamento { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Presupuesto { get; set; }

        public int TotalEmpleados { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateDepartamentoDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Presupuesto { get; set; }
    }

    public class UpdateDepartamentoDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Presupuesto { get; set; }
    }
}