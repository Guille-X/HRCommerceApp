using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Enums;
using HRCommerceApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRCommerceApp.Web.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IDepartamentoService _departamentoService;

        public EmpleadosController(IEmpleadoService empleadoService, IDepartamentoService departamentoService)
        {
            _empleadoService = empleadoService;
            _departamentoService = departamentoService;
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoService.GetAllAsync();
            return View(empleados);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Create()
        {
            await CargarDepartamentosEnViewBag();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Create(CreateEmpleadoDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoService.CreateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // CORREGIDO: Llamar al método que convierte a SelectListItem
            await CargarDepartamentosEnViewBag();
            return View(model);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            // CORREGIDO: Llamar al método que convierte a SelectListItem
            await CargarDepartamentosEnViewBag(empleado.DepartamentoId);

            var updateDto = new UpdateEmpleadoDto
            {
                Nombres = empleado.Nombres,
                Apellidos = empleado.Apellidos,
                Documento = empleado.Documento,
                FechaIngreso = empleado.FechaIngreso,
                SalarioActual = empleado.SalarioActual,
                Puesto = empleado.Puesto,
                Jerarquia = empleado.Jerarquia,
                DepartamentoId = empleado.DepartamentoId
            };

            return View(updateDto);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Edit(int id, UpdateEmpleadoDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoService.UpdateAsync(id, model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // CORREGIDO: Llamar al método que convierte a SelectListItem
            await CargarDepartamentosEnViewBag(model.DepartamentoId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _empleadoService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // MÉTODO AUXILIAR PARA CARGAR DEPARTAMENTOS CORRECTAMENTE
        private async Task CargarDepartamentosEnViewBag(int? departamentoSeleccionado = null)
        {
            var departamentos = await _departamentoService.GetAllAsync();
            ViewBag.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.IdDepartamento.ToString(),
                Text = d.Nombre,
                Selected = departamentoSeleccionado.HasValue && d.IdDepartamento == departamentoSeleccionado.Value
            }).ToList();
        }
    }
}