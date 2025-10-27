using HRCommerceApp.Core.DTOs.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> GetAllAsync();
        Task<EmpleadoDto> GetByIdAsync(int id);
        Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto createDto);
        Task<EmpleadoDto> UpdateAsync(int id, UpdateEmpleadoDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApplySalaryIncreaseAsync(int empleadoId, AumentoSalarialDto aumentoDto);
        Task<IEnumerable<EmpleadoDto>> GetByDepartamentoAsync(int departamentoId);
    }
}
