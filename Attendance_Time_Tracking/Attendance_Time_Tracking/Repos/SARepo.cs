using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace Attendance_Time_Tracking.Repos
{
    public interface ISARepo
    {
        public Task<AttendanceViewModel> ViewAllAttendance(DateOnly date);
        public Task<AttendanceViewModel> ViewAttendance(UserRole role, DateOnly date);
        public Task RecordAttendance(int id, AttendanceStatus status, DateOnly date);
        public Task ChangeAttendanceStatus(int id, AttendanceStatus status, DateOnly date);
        public Task<List<Permission>> GetPermissions(DateOnly date,UserRole role);
    }
    public class SARepo : ISARepo
    {
        readonly AttendanceContext db;

        public SARepo(AttendanceContext _db, IEmployeeRepo _empRepo)
        {
            db = _db;
        }

        public async Task<AttendanceViewModel> ViewAllAttendance(DateOnly date)
        {
            AttendanceViewModel attendance = new AttendanceViewModel();
            var users = await db.Users.Include(u => u.Attendances).ToListAsync();
            attendance.Present = users.Where(u => u.Attendances.Any(a => a.Date == date && a.Status != AttendanceStatus.Absent)).ToList();
            attendance.Absent = users.Where(u => !u.Attendances.Any(a => a.Date == date) || u.Attendances.Any(a => a.Date == date && a.Status == AttendanceStatus.Absent)).ToList();
            return attendance;
        }

        public async Task<AttendanceViewModel> ViewAttendance(UserRole role, DateOnly date)
        {
            AttendanceViewModel attendance = new AttendanceViewModel();
            var users = await db.Users.Include(u => u.Attendances).Where(u => u.Role == role).ToListAsync();
            attendance.Present = users.Where(u => u.Attendances.Any(a => a.Date == date && a.Status != AttendanceStatus.Absent)).ToList();
            attendance.Absent = users.Where(u => !u.Attendances.Any(a => a.Date == date) || u.Attendances.Any(a => a.Date == date && a.Status == AttendanceStatus.Absent)).ToList();
            return attendance;
        }

        public async Task RecordAttendance(int id, AttendanceStatus status, DateOnly date)
        {
            var attendance = new Attendance
            {
                UserId = id,
                Status = status,
                Date = date
            };
            db.Attendances.Add(attendance);
            await db.SaveChangesAsync();
        }

        public async Task ChangeAttendanceStatus(int id, AttendanceStatus status, DateOnly date)
        {
            var attendance = await db.Attendances.FindAsync(id, date);
            if (attendance == null)
            {
                await RecordAttendance(id, status, date);
            }
            else
            {
                attendance.Status = status;
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Permission>> GetPermissions(DateOnly date, UserRole role)
        {
            return await db.Permissions.Where(p => DateOnly.FromDateTime(p.Date) == date && p.Status == PermissionStatus.Approved).ToListAsync();
        }

    }
}
