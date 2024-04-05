using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance_Time_Tracking.Repos
{
    public interface IStudentRepo
    {
        Task<User> GetUserByID(int id);
        Task<List<Permission>> GetPermissionsByStudentID(int id);
        Task<Permission> DeletePermission(Permission p);
        Task<Permission> GetPermission(int id, DateTime date);
    }

    public class StudentRepo : IStudentRepo
    {
        private readonly AttendanceContext _db;

        public StudentRepo(AttendanceContext db)
        {
            _db = db;
        }

        public async Task<User> GetUserByID(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<List<Permission>> GetPermissionsByStudentID(int id)
        {
            return await _db.Permissions.Where(p => p.StdId == id).ToListAsync();
        }

        public async Task<Permission> DeletePermission(Permission p)
        {
            _db.Permissions.Remove(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task<Permission> GetPermission(int id, DateTime date)
        {
            List<Permission> permissions = await _db.Permissions.Where(p => p.StdId == id).ToListAsync();
            foreach (var permission in permissions)
            {
                if (permission.Date.Date == date.Date)
                {
                    return permission;
                }
            }
            return null;
        }
    }
}
