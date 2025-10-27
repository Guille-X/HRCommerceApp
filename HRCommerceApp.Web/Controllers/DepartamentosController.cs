using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.RRHH;
using HRCommerceApp.Core.Enums;

namespace HRCommerceApp.Web.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {
        private readonly IDepartamentoService _departamentoService;

        public DepartamentosController(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoService.GetAllAsync();
            return View(departamentos);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Details(int id)
        {
            var departamento = await _departamentoService.GetByIdAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Create(CreateDepartamentoDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departamentoService.CreateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Edit(int id)
        {
            var departamento = await _departamentoService.GetByIdAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateDepartamentoDto
            {
                Nombre = departamento.Nombre,
                Presupuesto = departamento.Presupuesto
            };

            return View(updateDto);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Administrador},{UserRole.Operador}")]
        public async Task<IActionResult> Edit(int id, UpdateDepartamentoDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departamentoService.UpdateAsync(id, model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Administrador)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departamentoService.DeleteAsync(id);
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