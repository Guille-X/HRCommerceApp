using HRCommerceApp.Core.DTOs.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Interfaces
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<DepartamentoDto>> GetAllAsync();
        Task<DepartamentoDto> GetByIdAsync(int id);
        Task<DepartamentoDto> CreateAsync(CreateDepartamentoDto createDto);
        Task<DepartamentoDto> UpdateAsync(int id, UpdateDepartamentoDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}