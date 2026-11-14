using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Departments
                .Include(d => d.Teachers)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => d.Name.Contains(search));
            }

            ViewData["Search"] = search;
            return View(await query.OrderBy(d => d.Name).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Кафедра успешно добавлена.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var department = await _context.Departments.FindAsync(id);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();
            if (!ModelState.IsValid) return View(department);

            _context.Update(department);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Изменения сохранены.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var department = await _context.Departments
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.Id == id);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return NotFound();

            if (department.Teachers.Any())
            {
                TempData["Error"] = "Нельзя удалить кафедру, пока к ней привязаны преподаватели.";
                return RedirectToAction(nameof(Index));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Кафедра удалена.";
            return RedirectToAction(nameof(Index));
        }
    }
}
