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
    public class DetalleVenta : IAuditable
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required]
        public int IdVenta { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Navigation properties
        [ForeignKey("IdVenta")]
        public virtual Venta Venta { get; set; }

        [ForeignKey("SKU")]
        public virtual Producto Producto { get; set; }

        // Auditoría - Implementación COMPLETA
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}