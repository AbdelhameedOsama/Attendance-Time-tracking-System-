using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_Time_Tracking.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Attendance_Time_Tracking.Data;
using System.Diagnostics;

namespace Attendance_Time_Tracking.Controllers
{
    [Authorize(Roles = "Student")]
    public class PermissionsController : Controller
    {
        private readonly AttendanceContext _context;

        public PermissionsController(AttendanceContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
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
            return View(permission);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var permissions = await _context.Permissions.Where(p => p.StdId == userId).ToListAsync();
            return View(permissions);
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







        [HttpPost, ActionName("DeleteConfirmed")]
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


        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}

