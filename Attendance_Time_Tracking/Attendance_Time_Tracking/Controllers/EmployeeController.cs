using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Attendance_Time_Tracking.Controllers
{
	[Authorize(Roles = "Employee")]
	public class EmployeeController : Controller
    {
        readonly IEmployeeRepo empRepo;
        
        public EmployeeController(IEmployeeRepo _empRepo)
        {
            empRepo = _empRepo;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAttendance(UserRole? role = null)
        {
            AttendanceViewModel attendance;
            if (role == null)
            {
                attendance = await empRepo.ViewAllAttendance();
                ViewBag.Role = null;
            }
            else
            {
                attendance = await empRepo.ViewAttendance(role.Value);
                ViewBag.Role = (int)role.Value;
            }
            return PartialView(attendance);
        }

        public async Task<IActionResult> RefreshAttendance(UserRole? role = null)
        {
            AttendanceViewModel attendance;
            if (role == null)
            {
                attendance = await empRepo.ViewAllAttendance();
                ViewBag.Role = null;
            }
            else
            {
                attendance = await empRepo.ViewAttendance(role.Value);
                ViewBag.Role = (int)role.Value;
            }
            return View(attendance);
        }

        public async Task<IActionResult> TakeAttendance(int id, UserRole? role)
        {
            await empRepo.RecordAttendance(id);
            return RedirectToAction("RefreshAttendance", role);
        }

        public async Task<IActionResult> AddDepartureTime(int id, UserRole? role)
        {
            await empRepo.AddDepartureTime(id);
            return RedirectToAction("RefreshAttendance", role);
        }

        public async Task<IActionResult> RemoveAttendance(int id, UserRole? role)
        {
            await empRepo.RemoveAttendance(id);
            return RedirectToAction("RefreshAttendance", role);
        }
    }
}
