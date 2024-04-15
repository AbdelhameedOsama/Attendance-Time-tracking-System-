using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Migrations;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Attendance_Time_Tracking.Repos
{
    public interface IEmployeeRepo
    {
        public Task<AttendanceViewModel> ViewAllAttendance();
        public Task<AttendanceViewModel> ViewAttendance(UserRole role);
        public Task<bool> RecordAttendance(int id);
        public Task<bool> AddDepartureTime(int id);
        public Task<bool> RemoveAttendance(int id);
        public List<Employee> GetAllEmployees();
        public void AddEmpployee(Employee emp);
        public void DeleteEmployee(int id);
        public void UpdateEmp(Employee employee);

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
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var users = await db.Users.Include(u => u.Attendances).ToListAsync();
            attendance.Present = users.Where(u => u.Attendances.Any(a => a.Date == today)).ToList();
            attendance.Absent = users.Where(u => !u.Attendances.Any(a => a.Date == today)).ToList();
            return attendance;
        }

        public async Task<AttendanceViewModel> ViewAttendance(UserRole role)
        {
            AttendanceViewModel attendance = new AttendanceViewModel();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var users = await db.Users.Include(u => u.Attendances).Where(u => u.Role == role).ToListAsync();
            attendance.Present = users.Where(u => u.Attendances.Any(a => a.Date == today)).ToList();
            attendance.Absent = users.Where(u => !u.Attendances.Any(a => a.Date == today)).ToList();
            return attendance;
        }

        public async Task<bool> RecordAttendance(int id)
        {
            DateTime time = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(time);
            AttendanceStatus status;
			TimeSpan startTime = db.Schedules.Where(s => s.Date == today).Select(s => s.Start_Time).FirstOrDefault();
			TimeSpan lateTime = startTime.Add(TimeSpan.FromMinutes(15));
            var user = await db.Users.Include(u => u.Attendances).FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return false;
            }
            if (user.Attendances.Any(a => a.Date == today))
            {
                return false;
            }
            if (time <= DateTime.Today.Add(lateTime))
            {
                status = AttendanceStatus.Present;
            }
            else
            {
                status = AttendanceStatus.Late;
            }
            user.Attendances.Add(new Attendance { UserId = id, Date = today, Status = status, Arrival_Time = TimeOnly.FromDateTime(time), Departure_Time = null });
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDepartureTime(int id)
        {
            DateTime time = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(time);
            var user = await db.Users.Include(u => u.Attendances).FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return false;
            }
            var attendance = user.Attendances.FirstOrDefault(a => a.Date == today);
            if (attendance == null)
            {
                return false;
            }
            attendance.Departure_Time = TimeOnly.FromDateTime(time);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAttendance(int id)
        {
            DateTime time = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(time);
            var user = await db.Users.Include(u => u.Attendances).FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return false;
            }
            var attendance = user.Attendances.FirstOrDefault(a => a.Date == today);
            if (attendance == null)
            {
                return false;
            }
            db.Attendances.Remove(attendance);
            await db.SaveChangesAsync();
            return true;
        }
        public List<Employee> GetAllEmployees()
        {
            return db.Employees.ToList();
        }
        public void DeleteEmployee(int id)
        {
            var emp = db.Employees.FirstOrDefault(a => a.ID == id);
            db.Users.Remove(emp);
            db.SaveChanges();
        }

        public void AddEmpployee(Employee emp)
        {
            db.Employees.Add(emp);
            db.SaveChanges();
        }
        public void UpdateEmp(Employee employee)
        {
            var existingUser = db.Users.FirstOrDefault(a => a.ID == employee.ID);
            if (existingUser != null)
            {
                existingUser.Email = employee.Email;
                existingUser.Password = employee.Password;
                existingUser.Address= employee.Address;
                existingUser.Phone= employee.Phone;
                existingUser.Name= employee.Name;
                /* db.Entry(existingUser).State = EntityState.Detached;
                 db.Entry(user).State = EntityState.Modified; */

                db.SaveChanges();
            }
        }
    }
}
