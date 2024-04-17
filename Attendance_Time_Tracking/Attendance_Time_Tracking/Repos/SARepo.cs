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
        public Task<int> GetDegree(int studentId);
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
                        await RecordAttendance(student.ID, AttendanceStatus.Present, date);
                    }
                    else
                    {
                        await RecordAttendance(student.ID, AttendanceStatus.Absent, date);
                    }
                }
            }
        }

        public async Task<int> GetDegree(int studentId)
        {
            int Degree = 250;
            var student = await db.Students.FirstOrDefaultAsync(s => s.ID == studentId);
            int counter = 0;
            int counter5 = 0;
            int counter10 = 0;
            int counter15 = 0;
            int counter20 = 0;
            int counter25 = 0;
            if (student == null)
            {
                return -1;
            }
            List<Attendance> absentDays = await db.Attendances.Where(a => a.UserId == studentId && a.Status != AttendanceStatus.Present).ToListAsync();
            List<Permission> permissions = await db.Permissions.Where(p => p.StdId == studentId && p.Status == PermissionStatus.Approved).ToListAsync();

            foreach (var day in absentDays)
            {
                if (counter == 0)
                {
                    counter++;
                    continue;
                }

                if (permissions.Any(p => DateOnly.FromDateTime(p.Date) == day.Date))
                {
                    if (counter5 <= 3)
                    {
                        counter5++;
                        Degree -= 5;
                    }
                    else if (counter10 <= 3)
                    {
                        counter10++;
                        Degree -= 10;
                    }
                    else if (counter15 <= 3)
                    {
                        counter15++;
                        Degree -= 15;
                    }
                    else if (counter20 <= 3)
                    {
                        counter20++;
                        Degree -= 20;
                    }
                    else
                    {
                        counter25++;
                        Degree -= 25;
                    }
                }
                else
                {
                    counter25++;
                    Degree -= 25;
                }
            }
            return Degree;
        }
    }
}
