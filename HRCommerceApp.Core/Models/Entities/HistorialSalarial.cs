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
    public class HistorialSalarial : IAuditable
    {
        [Key]
        public int IdHistorial { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalarioAnterior { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NuevoSalario { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Aumento { get; set; }

        [Required]
        public decimal PorcentajeAumento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaAumento { get; set; }

        [StringLength(500)]
        public string Motivo { get; set; }

        // Navigation properties
        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        // Auditoría - Implementación COMPLETA
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}