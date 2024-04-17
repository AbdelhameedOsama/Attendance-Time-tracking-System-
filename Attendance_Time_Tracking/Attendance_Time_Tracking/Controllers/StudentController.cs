using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Attendance_Time_Tracking.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepo _studentRepo;
        private readonly AttendanceContext _context;
        private readonly ISARepo _saRepo;

        public StudentController(IStudentRepo studentRepo, AttendanceContext context, ISARepo saRepo)
        {
            _studentRepo = studentRepo;
            _context = context;
            _saRepo = saRepo;

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
                    var emailExists = await _context.Users.AnyAsync(u => u.Email == student.Email && u.ID != student.ID);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "The email is already in use.");
                        return View("~/Views/Student/Edit.cshtml", student);
                    }

                    // Validate name (only letters and spaces)
                    if (!Regex.IsMatch(student.Name, "^[a-zA-Z\\s]+$"))
                    {
                        ModelState.AddModelError("Name", "Name must contain only letters and spaces.");
                        return View("~/Views/Student/Edit.cshtml", student);
                    }
                    // Validate phone number (Egyptian format)
                    var phoneString = student.Phone.ToString();
                    if (!Regex.IsMatch(phoneString, @"^01[0125]\d{8}$"))
                    {
                        ModelState.AddModelError("Phone", "Invalid Egyptian phone number format.");
                        return View("~/Views/Student/Edit.cshtml", student);
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

        public async Task<IActionResult> Schedule(DateTime? FromDate)
        {
            var userId = GetCurrentUserId();
            var student = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.ID == userId);

            if (student == null)
            {
                return NotFound();
            }

            IQueryable<Schedule> schedulesQuery = _context.Schedules
                .Include(s => s.Track)
                .Include(s => s.Supervisor)
                .Where(s => s.TrackId == student.TrackId);

            var startDate = FromDate.HasValue ? DateOnly.FromDateTime(FromDate.Value) : DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(7);

            schedulesQuery = schedulesQuery.Where(s => s.Date >= startDate && s.Date < endDate);

            var schedules = await schedulesQuery.OrderBy(s => s.Date).ToListAsync();

            return View("~/Views/Student/View.cshtml", schedules);
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
                var userId = GetCurrentUserId();
                var student = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.ID == userId);

                // Check if the reason is empty
                if (string.IsNullOrEmpty(permission.Reason))
                {
                    ModelState.AddModelError("Reason", "The Reason field is required.");
                    return View("~/Views/Student/Create.cshtml", permission);
                }

                // Create the permission
                permission.StdId = userId;
                permission.Status = PermissionStatus.Pending;

                // Get the track for the student
                var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Instructors.Any(i => i.ID == userId));
                if (track != null)
                {
                    permission.SupId = track.SupID;
                }
                else
                {
                    permission.SupId = 0;
                }

                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Student/Create.cshtml", permission);
        }

        public async Task<IActionResult> GetDegree()
        {
            var userId = GetCurrentUserId();
            var degree = await _saRepo.GetDegree(userId);
            if (degree == null)
            {
                return NotFound();
            }

            return View("~/Views/Student/Degree.cshtml", degree);
        }







        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var permissions = await _context.Permissions.Where(p => p.StdId == userId).ToListAsync();
            return View("~/Views/Student/Index.cshtml", permissions);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var userId = GetCurrentUserId();
        //    IEnumerable<Permission> permissions = await _context.Permissions.Where(p => p.StdId == userId).ToListAsync();
        //    var degree = await _saRepo.GetDegree(userId);
        //    return View("~/Views/Student/Index.cshtml", Tuple.Create(permissions, degree));
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DateTime date)
        {
            var permission = await _studentRepo.GetPermission(id, date);
            if (permission == null)
            {
                return NotFound();
            }

            await _studentRepo.DeletePermission(permission);
            return RedirectToAction(nameof(Index));
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
