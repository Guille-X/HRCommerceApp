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
    public class Producto : IAuditable
    {
        [Key]
        [StringLength(50)]
        public string SKU { get; set; }

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public int Existencia { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }

        // Auditoría - Implementación COMPLETA
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}