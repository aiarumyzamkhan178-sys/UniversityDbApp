using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                DepartmentsCount = await _context.Departments.CountAsync(),
                TeachersCount = await _context.Teachers.CountAsync(),
                RecentTeachers = await _context.Teachers
                    .Include(t => t.Department)
                    .OrderByDescending(t => t.Id)
                    .Take(5)
                    .ToListAsync(),
                DepartmentStatistics = await _context.Departments
                    .Select(d => new DepartmentStatistic
                    {
                        Name = d.Name,
                        TeachersCount = d.Teachers.Count
                    })
                    .OrderByDescending(d => d.TeachersCount)
                    .Take(6)
                    .ToListAsync()
            };

            model.RecentTeachersCount = model.RecentTeachers.Count;
            return View(model);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
