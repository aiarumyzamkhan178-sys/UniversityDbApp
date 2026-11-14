using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? departmentId)
        {
            var query = _context.Teachers.Include(t => t.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.FullName.Contains(search));

            if (departmentId.HasValue)
                query = query.Where(t => t.DepartmentId == departmentId.Value);

            await LoadDepartments(departmentId);
            ViewData["Search"] = search;
            return View(await query.OrderBy(t => t.FullName).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            await LoadDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Преподаватель успешно добавлен.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            await LoadDepartments(teacher.DepartmentId);
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }

            _context.Update(teacher);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Данные преподавателя обновлены.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);
            return teacher == null ? NotFound() : View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Преподаватель удалён.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDepartments(int? selectedId = null)
        {
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();

            ViewBag.DepartmentId = new SelectList(
                departments, "Id", "Name", selectedId);
        }
    }
}
