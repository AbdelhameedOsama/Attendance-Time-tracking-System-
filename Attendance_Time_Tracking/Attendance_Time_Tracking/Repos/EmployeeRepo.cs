using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Repos
{
    public interface IEmployeeRepo
    {
        public Task<AttendanceViewModel> ViewAllAttendance();
    }

    public class EmployeeRepo : IEmployeeRepo
    {
        readonly AttendanceContext db;

        public EmployeeRepo(AttendanceContext _db)
        {
            db = _db;
        }

        public async Task<AttendanceViewModel> ViewAllAttendance()
        {
            AttendanceViewModel attendance = new AttendanceViewModel();
            attendance.Present = await db.Users.Include(u => u.Attendances).Where(u => u.Attendances.Any(a => a.Date.DayOfYear == DateTime.Today.DayOfYear)).ToListAsync();
            attendance.Absent = await db.Users.Include(u => u.Attendances).Where(u => !u.Attendances.Any(a => a.Date.Date == DateTime.Today.Date)).ToListAsync();
            return attendance;
        }
    }
}
