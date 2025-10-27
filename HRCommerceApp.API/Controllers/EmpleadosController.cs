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
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadosController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var empleados = await _empleadoService.GetAllAsync();
                return Ok(empleados);
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
                var empleado = await _empleadoService.GetByIdAsync(id);
                if (empleado == null)
                    return NotFound(new { message = "Empleado no encontrado" });

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Create([FromBody] CreateEmpleadoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var empleado = await _empleadoService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = empleado.IdEmpleado }, empleado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmpleadoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var empleado = await _empleadoService.UpdateAsync(id, updateDto);
                if (empleado == null)
                    return NotFound(new { message = "Empleado no encontrado" });

                return Ok(empleado);
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
                var result = await _empleadoService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = "Empleado no encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/aumento-salarial")]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> ApplySalaryIncrease(int id, [FromBody] AumentoSalarialDto aumentoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _empleadoService.ApplySalaryIncreaseAsync(id, aumentoDto);
                if (!result)
                    return NotFound(new { message = "Empleado no encontrado" });

                return Ok(new { message = "Aumento salarial aplicado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("departamento/{departamentoId}")]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> GetByDepartamento(int departamentoId)
        {
            try
            {
                var empleados = await _empleadoService.GetByDepartamentoAsync(departamentoId);
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}