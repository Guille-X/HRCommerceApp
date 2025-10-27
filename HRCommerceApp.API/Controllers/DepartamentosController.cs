using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Enums;

namespace HRCommerceApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoService _departamentoService;

        public DepartamentosController(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var departamentos = await _departamentoService.GetAllAsync();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var departamento = await _departamentoService.GetByIdAsync(id);
                if (departamento == null)
                    return NotFound(new { message = "Departamento no encontrado" });

                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Create([FromBody] CreateDepartamentoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var departamento = await _departamentoService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = departamento.IdDepartamento }, departamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartamentoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var departamento = await _departamentoService.UpdateAsync(id, updateDto);
                if (departamento == null)
                    return NotFound(new { message = "Departamento no encontrado" });

                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _departamentoService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = "Departamento no encontrado" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}