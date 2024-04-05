using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly AttendanceContext _context;

        public StudentController(AttendanceContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        // Existing Details action method
        public async Task<IActionResult> Details()
        {
            var currentUserId = GetCurrentUserId();
            var student = await _context.Users
                .OfType<Student>()
                .AsNoTracking() // Add this line
                .Include(s => s.Permissions)
                .FirstOrDefaultAsync(s => s.ID == currentUserId);

            if (student == null)
            {
                return NotFound();
            }

            return View("~/Views/Student/Details.cshtml", student);
        }

        // New Edit action method
        public async Task<IActionResult> Edit()
        {
            var currentUserId = GetCurrentUserId();
            var student = await _context.Users
                .OfType<Student>()
                .FirstOrDefaultAsync(s => s.ID == currentUserId);

            if (student == null)
            {
                return NotFound();
            }

            return View("~/Views/Student/Edit.cshtml", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            var currentUserId = GetCurrentUserId();
            if (student.ID != currentUserId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var studentInDb = await _context.Users
                        .OfType<Student>()
                        .FirstOrDefaultAsync(s => s.ID == currentUserId);

                    if (studentInDb == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the studentInDb object
                    studentInDb.Email = student.Email;
                    studentInDb.Password = student.Password;
                    studentInDb.Name = student.Name;
                    studentInDb.Address = student.Address;
                    studentInDb.Phone = student.Phone;
                    studentInDb.University = student.University;
                    studentInDb.Faculty = student.Faculty;
                    studentInDb.Specialization = student.Specialization;
                    studentInDb.Graduation_year = student.Graduation_year;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            return View("~/Views/Student/Edit.cshtml", student);
        }

        public async Task<IActionResult> Schedule()
        {
            var userId = GetCurrentUserId();
            var student = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.ID == userId);

            if (student == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Track)
                .Include(s => s.Supervisor)
                .Where(s => s.TrackId == student.TrackId && s.Date >= DateTime.Today)
                .OrderBy(s => s.Date)
                .ToListAsync();

            return View("~/Views/Student/View.cshtml", schedule);
        }

        public IActionResult Create()
        {
            return View("~/Views/Student/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                // Get current user ID (assuming it's stored in StdId)
                permission.StdId = GetCurrentUserId();
                permission.Date = DateTime.Now;
                permission.Status = PermissionStatus.Pending;

                // Add SupervisorId from the form submission
                permission.SupId = permission.SupId;

                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Student/Create.cshtml", permission);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var permissions = await _context.Permissions.Where(p => p.StdId == userId).ToListAsync();
            return View("~/Views/Student/Index.cshtml", permissions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int stdId, string date)
        {
            var parsedDate = DateTime.Parse(date);

            // Check if permission belongs to current student
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.StdId == stdId && p.Date == parsedDate);

            if (permission == null)
            {
                // Permission not found or doesn't belong to current student
                return NotFound();
            }

            // Delete permission
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int stdId, string date)
        {
            try
            {
                // Parse the date string back to DateTime
                var parsedDate = DateTime.Parse(date);
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.StdId == stdId && p.Date == parsedDate);

                if (permission == null)
                {
                    return NotFound();
                }

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log or handle the exception (e.g., display error message)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        public async Task<IActionResult> Attendance()
        {
            var userId = GetCurrentUserId();
            var attendances = await _context.Attendances
                .Where(a => a.UserId == userId)
                .ToListAsync();
            return View("~/Views/Student/Attendance.cshtml", attendances);
        }

        [HttpPost]
        public async Task<IActionResult> Attendance(DateTime FromDate)
        {
            ViewData["Role"] = User.FindFirst(ClaimTypes.Role).Value;
            var userId = GetCurrentUserId();
            var attendances = await _context.Attendances
                .Where(a => a.UserId == userId && a.Date >= DateOnly.FromDateTime(FromDate))
                .OrderBy(a => a.Date)
                .Take(7)
                .ToListAsync();

            return View("~/Views/Student/Attendance.cshtml", attendances);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
