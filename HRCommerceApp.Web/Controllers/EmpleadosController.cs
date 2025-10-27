using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Enums;

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
            ViewBag.Departamentos = await _departamentoService.GetAllAsync();
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

            ViewBag.Departamentos = await _departamentoService.GetAllAsync();
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

            ViewBag.Departamentos = await _departamentoService.GetAllAsync();

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

            ViewBag.Departamentos = await _departamentoService.GetAllAsync();
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
    }
}