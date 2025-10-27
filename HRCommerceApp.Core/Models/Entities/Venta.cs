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
    public class Venta : IAuditable
    {
        [Key]
        public int IdVenta { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        // Navigation properties
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

        // Auditoría - Implementación COMPLETA
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}