using HRCommerceApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Models.Entities
{
    public class Empleado : IAuditable
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; } // CUI

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioActual { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaUltimoAumento { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaBaja { get; set; }

        [Required]
        [StringLength(100)]
        public string Puesto { get; set; }

        [Required]
        [StringLength(50)]
        public string Jerarquia { get; set; }

        // Foreign Key
        public int DepartamentoId { get; set; }

        // Navigation properties
        [ForeignKey("DepartamentoId")]
        public virtual Departamento Departamento { get; set; }

        // Historial Salarial (Plus)
        public virtual ICollection<HistorialSalarial> HistorialSalarial { get; set; } = new List<HistorialSalarial>();

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}
