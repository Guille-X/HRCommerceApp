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
    public class DepartamentoService : IDepartamentoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Departamento> _departamentoRepository;

        public DepartamentoService(ApplicationDbContext context, IRepository<Departamento> departamentoRepository)
        {
            _context = context;
            _departamentoRepository = departamentoRepository;
        }

        public async Task<IEnumerable<DepartamentoDto>> GetAllAsync()
        {
            var departamentos = await _context.Departamentos
                .Include(d => d.Empleados)
                .ToListAsync();

            return departamentos.Select(d => new DepartamentoDto
            {
                IdDepartamento = d.IdDepartamento,
                Nombre = d.Nombre,
                Presupuesto = d.Presupuesto,
                TotalEmpleados = d.Empleados.Count(e => e.FechaBaja == null),
                CreatedAt = d.CreatedAt
            });
        }

        public async Task<DepartamentoDto> GetByIdAsync(int id)
        {
            var departamento = await _context.Departamentos
                .Include(d => d.Empleados)
                .FirstOrDefaultAsync(d => d.IdDepartamento == id);

            if (departamento == null) return null;

            return new DepartamentoDto
            {
                IdDepartamento = departamento.IdDepartamento,
                Nombre = departamento.Nombre,
                Presupuesto = departamento.Presupuesto,
                TotalEmpleados = departamento.Empleados.Count(e => e.FechaBaja == null),
                CreatedAt = departamento.CreatedAt
            };
        }

        public async Task<DepartamentoDto> CreateAsync(CreateDepartamentoDto createDto)
        {
            var departamento = new Departamento
            {
                Nombre = createDto.Nombre,
                Presupuesto = createDto.Presupuesto,
                CreatedBy = "system"
            };

            await _departamentoRepository.AddAsync(departamento);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(departamento.IdDepartamento);
        }

        public async Task<DepartamentoDto> UpdateAsync(int id, UpdateDepartamentoDto updateDto)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);
            if (departamento == null) return null;

            departamento.Nombre = updateDto.Nombre;
            departamento.Presupuesto = updateDto.Presupuesto;
            departamento.UpdatedAt = DateTime.UtcNow;

            _departamentoRepository.Update(departamento);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);
            if (departamento == null) return false;

            // Verificar si tiene empleados activos
            var tieneEmpleadosActivos = await _context.Empleados
                .AnyAsync(e => e.DepartamentoId == id && e.FechaBaja == null);

            if (tieneEmpleadosActivos)
                throw new InvalidOperationException("No se puede eliminar el departamento porque tiene empleados activos.");

            _departamentoRepository.Remove(departamento);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
