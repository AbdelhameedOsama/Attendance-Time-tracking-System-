using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace Attendance_Time_Tracking.Repos
{
    public interface IStudentRepo
    {
        Task<User> GetUserByID(int id);
        Task<List<Permission>> GetPermissionsByStudentID(int id);
        Task<Permission> DeletePermission(Permission p);
        Task<Permission> GetPermission(int id, DateTime date);
        public List<Student> GetAll();
        public Student GetStudentByID(int id);
        public void DeleteStudent(int id);
        public void Add(Student std);
        public void UpdateStd(Student std);

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
        public List<Student> GetAll()
        {
            return db.Students.ToList();
        }
        public Student GetStudentByID(int id)
        {
            return db.Students.FirstOrDefault(a=>a.ID == id);
        }
        public void DeleteStudent(int id)
        {
            var stdPermission = db.Permissions.FirstOrDefault(a => a.StdId==id);
            if (stdPermission!=null)
            {
                db.Permissions.Remove(stdPermission);
               
            }
            var stdAttendance = db.Attendances.Where(a => a.User.ID == id).ToList();
            foreach(var attendance in stdAttendance)
            {
                if (attendance!=null)
                {

                    db.Attendances.Remove(attendance);
                }
            }
            var std = db.Students.FirstOrDefault(a => a.ID ==id);
            db.Students.Remove(std);
            db.SaveChanges();
        }

        public void Add(Student std)
        {
            db.Students.Add(std);
            db.SaveChanges();
        }
        public void UpdateStd(Student std)
        {
            var existingStudent = db.Students.FirstOrDefault(a => a.ID == std.ID);
            if (existingStudent != null)
            {
                db.Entry(existingStudent).State = EntityState.Detached; // Detach the existing entity
                db.Entry(std).State = EntityState.Modified; // Attach and mark as modified
                db.SaveChanges();
            }
        }


    }
}
