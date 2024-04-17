using Attendance_Time_Tracking.Models;
using Attendance_Time_Tracking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Attendance_Time_Tracking.Controllers
{
	[Authorize(Roles = "Employee")]
	public class StudentAffairsController : Controller
    {
        readonly ISARepo SARepo;
        readonly IEmployeeRepo empRepo;

        public StudentAffairsController(ISARepo _SARepo, IEmployeeRepo _empRepo)
        {
            SARepo = _SARepo;
            empRepo = _empRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAttendance(DateOnly date, UserRole? role = null)
        {
            AttendanceViewModel attendance;
            if (date == DateOnly.MinValue)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            if (role == null)
            {
                attendance = await SARepo.ViewAllAttendance(date);
                ViewBag.Role = null;
            }
            else
            {
                attendance = await SARepo.ViewAttendance(role.Value, date);
                ViewBag.Role = (int)role.Value;
            }
            ViewBag.Date = date;
            ViewBag.degree = await SARepo.GetDegree(17);
            return View(attendance);
        }

        public async Task<IActionResult> TakeAttendance(int id, UserRole? role)
        {
            await empRepo.RecordAttendance(id);
            return RedirectToAction("ViewAttendance", role);
        }

        public async Task<IActionResult> AddDepartureTime(int id, UserRole? role)
        {
            await empRepo.AddDepartureTime(id);
            return RedirectToAction("ViewAttendance", role);
        }

        public async Task<IActionResult> RemoveAttendance(int id, UserRole? role)
        {
            await empRepo.RemoveAttendance(id);
            return RedirectToAction("ViewAttendance", role);
        }

        public async Task<IActionResult> SaveChangeStatus(int id, UserRole? role, AttendanceStatus status, DateOnly date)
        {
            await SARepo.ChangeAttendanceStatus(id, status, date);
            return RedirectToAction("ViewAttendance", new { date, role });
        }

        public async Task<IActionResult> ViewPermissions(DateOnly date, UserRole? role = UserRole.Student)
        {
            if (date == DateOnly.MinValue)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }

            var permissions = await SARepo.GetPermissions(date, role.Value);
            ViewBag.Date = date;
            ViewBag.Role = role;
            return View(permissions);
        }

        public async Task<IActionResult> AutoFinishAttendance(DateOnly date)
        {
            await SARepo.AutoFinishAttendance(date);
            return RedirectToAction("ViewAttendance", new { date });
        }
    }
}
