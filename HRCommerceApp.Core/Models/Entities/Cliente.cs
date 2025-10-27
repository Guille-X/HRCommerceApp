using HRCommerceApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HRCommerceApp.Core.Models.Entities
{
    public class Cliente : IAuditable
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(20)]
        public string Nit { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        // Auditoría - Implementación COMPLETA de IAuditable
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
    }
}