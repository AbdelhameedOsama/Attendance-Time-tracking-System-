using Attendance_Time_Tracking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AttendanceController : Controller
{
    private readonly AttendanceContext _context;

    public AttendanceController(AttendanceContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Attendance()
    {
        var userId = GetCurrentUserId();
        var attendances = await _context.Attendances
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return View("~/Views/Permissions/Attendance.cshtml", attendances);
    }



    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
