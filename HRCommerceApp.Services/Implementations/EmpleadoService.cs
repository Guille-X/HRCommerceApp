using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.Models.Entities;
using HRCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Services.Implementations
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Empleado> _empleadoRepository;

        public EmpleadoService(ApplicationDbContext context, IRepository<Empleado> empleadoRepository)
        {
            _context = context;
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
        {
            var empleados = await _context.Empleados
                .Include(e => e.Departamento)
                .Where(e => e.FechaBaja == null)
                .ToListAsync();

            return empleados.Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = e.Nombres,
                Apellidos = e.Apellidos,
                Documento = e.Documento,
                FechaIngreso = e.FechaIngreso,
                SalarioActual = e.SalarioActual,
                FechaUltimoAumento = e.FechaUltimoAumento,
                FechaBaja = e.FechaBaja,
                Puesto = e.Puesto,
                Jerarquia = e.Jerarquia,
                DepartamentoId = e.DepartamentoId,
                DepartamentoNombre = e.Departamento.Nombre,
                CreatedAt = e.CreatedAt
            });
        }

        public async Task<EmpleadoDto> GetByIdAsync(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Departamento)
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null) return null;

            return new EmpleadoDto
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombres = empleado.Nombres,
                Apellidos = empleado.Apellidos,
                Documento = empleado.Documento,
                FechaIngreso = empleado.FechaIngreso,
                SalarioActual = empleado.SalarioActual,
                FechaUltimoAumento = empleado.FechaUltimoAumento,
                FechaBaja = empleado.FechaBaja,
                Puesto = empleado.Puesto,
                Jerarquia = empleado.Jerarquia,
                DepartamentoId = empleado.DepartamentoId,
                DepartamentoNombre = empleado.Departamento.Nombre,
                CreatedAt = empleado.CreatedAt
            };
        }

        public async Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto createDto)
        {
            var empleado = new Empleado
            {
                Nombres = createDto.Nombres,
                Apellidos = createDto.Apellidos,
                Documento = createDto.Documento,
                FechaIngreso = createDto.FechaIngreso,
                SalarioActual = createDto.SalarioActual,
                FechaUltimoAumento = createDto.FechaIngreso, // Misma fecha de ingreso inicial
                Puesto = createDto.Puesto,
                Jerarquia = createDto.Jerarquia,
                DepartamentoId = createDto.DepartamentoId,
                CreatedBy = "system"
            };

            await _empleadoRepository.AddAsync(empleado);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(empleado.IdEmpleado);
        }

        public async Task<EmpleadoDto> UpdateAsync(int id, UpdateEmpleadoDto updateDto)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null) return null;

            empleado.Nombres = updateDto.Nombres;
            empleado.Apellidos = updateDto.Apellidos;
            empleado.Documento = updateDto.Documento;
            empleado.FechaIngreso = updateDto.FechaIngreso;
            empleado.Puesto = updateDto.Puesto;
            empleado.Jerarquia = updateDto.Jerarquia;
            empleado.DepartamentoId = updateDto.DepartamentoId;
            empleado.UpdatedAt = DateTime.UtcNow;

            _empleadoRepository.Update(empleado);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null) return false;

            empleado.FechaBaja = DateTime.UtcNow;
            _empleadoRepository.Update(empleado);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApplySalaryIncreaseAsync(int empleadoId, AumentoSalarialDto aumentoDto)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(empleadoId);
            if (empleado == null) return false;

            // Crear historial antes de actualizar
            var historial = new HistorialSalarial
            {
                EmpleadoId = empleadoId,
                SalarioAnterior = empleado.SalarioActual,
                NuevoSalario = aumentoDto.NuevoSalario,
                Aumento = aumentoDto.NuevoSalario - empleado.SalarioActual,
                PorcentajeAumento = ((aumentoDto.NuevoSalario - empleado.SalarioActual) / empleado.SalarioActual) * 100,
                FechaAumento = aumentoDto.FechaAumento,
                Motivo = aumentoDto.Motivo,
                CreatedBy = "system"
            };

            // Actualizar empleado
            empleado.SalarioActual = aumentoDto.NuevoSalario;
            empleado.FechaUltimoAumento = aumentoDto.FechaAumento;
            empleado.UpdatedAt = DateTime.UtcNow;

            _context.HistorialSalarial.Add(historial);
            _empleadoRepository.Update(empleado);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetByDepartamentoAsync(int departamentoId)
        {
            var empleados = await _context.Empleados
                .Include(e => e.Departamento)
                .Where(e => e.DepartamentoId == departamentoId && e.FechaBaja == null)
                .ToListAsync();

            return empleados.Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = e.Nombres,
                Apellidos = e.Apellidos,
                Documento = e.Documento,
                FechaIngreso = e.FechaIngreso,
                SalarioActual = e.SalarioActual,
                FechaUltimoAumento = e.FechaUltimoAumento,
                Puesto = e.Puesto,
                Jerarquia = e.Jerarquia,
                DepartamentoId = e.DepartamentoId,
                DepartamentoNombre = e.Departamento.Nombre,
                CreatedAt = e.CreatedAt
            });
        }
    }
}
