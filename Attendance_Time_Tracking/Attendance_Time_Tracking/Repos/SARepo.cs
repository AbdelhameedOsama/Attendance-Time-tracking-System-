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
        public Task AutoFinishAttendance(DateOnly date);
        public Task<Dictionary<string, int>> GetDegree(int studentId);
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

        public async Task AutoFinishAttendance(DateOnly date)
        {
            List<Permission> permissions = await db.Permissions.Where(p => DateOnly.FromDateTime(p.Date) == date && p.Status == PermissionStatus.Approved).ToListAsync();
            List<Attendance> attendances = await db.Attendances.Where(a => a.Date == date).ToListAsync();
            List<Schedule> schedules = await db.Schedules.Where(s => s.Date == date).ToListAsync();
            List<int> tracks = schedules.Select(s => s.TrackId).ToList();
            List<Student> students = await db.Students.Where(s => tracks.Contains(s.TrackId)).ToListAsync();

            foreach (var student in students)
            {
                if (!attendances.Any(a => a.UserId == student.ID))
                {
                    if (permissions.Any(p => p.StdId == student.ID))
                    {
                        AttendanceStatus status = permissions.First(p => p.StdId == student.ID).Type switch
                        {
                            PermissionTypes.Absence => AttendanceStatus.Absent,
                            PermissionTypes.Late_Arrival => AttendanceStatus.Late,
                            _ => AttendanceStatus.Present
                        };
                        await RecordAttendance(student.ID, status, date);
                    }
                    else
                    {
                        await RecordAttendance(student.ID, AttendanceStatus.Absent, date);
                    }
                }
            }
        }

        public async Task<Dictionary<string, int>> GetDegree(int studentId)
        {
            int Degree = 250;
            var student = await db.Students.FirstOrDefaultAsync(s => s.ID == studentId);
            if (student == null)
            {
                return null;
            }
            List<Attendance> absentDays = await db.Attendances.Where(a => a.UserId == studentId && a.Status != AttendanceStatus.Present).ToListAsync();
            List<Permission> permissions = await db.Permissions.Where(p => p.StdId == studentId && p.Status == PermissionStatus.Approved ).ToListAsync();

            Degree -= (absentDays.Count - permissions.Count) * 25;

            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Degree", Degree);
            result.Add("AbsentDays", absentDays.Count);
            result.Add("5", 0);
            result.Add("10", 0);
            result.Add("15", 0);
            result.Add("20", 0);
            result.Add("25", (absentDays.Count - permissions.Count));

            for (int i = 0; i < permissions.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else if (i <= 3)
                {
                    result["5"]++;
                    result["Degree"] -= 5;
                }
                else if (i <= 6)
                {
                    result["10"]++;
                    result["Degree"] -= 10;
                }
                else if (i <= 9)
                {
                    result["15"]++;
                    result["Degree"] -= 15;
                }
                else if (i <= 12)
                {
                    result["20"]++;
                    result["Degree"] -= 20;
                }
                else
                {
                    result["25"]++;
                    result["Degree"] -= 25;
                }
            }

            return result;
        }
    }
}
