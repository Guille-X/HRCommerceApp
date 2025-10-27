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
    public class Departamento : IAuditable
    {
        [Key]
        public int IdDepartamento { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Presupuesto { get; set; }

        // Navigation properties
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}
