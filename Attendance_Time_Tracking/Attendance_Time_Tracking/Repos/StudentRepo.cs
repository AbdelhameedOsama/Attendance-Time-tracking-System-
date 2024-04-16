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
        public void AddStudents(List<Student> students);

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
            return _db.Students.ToList();
        }
        public Student GetStudentByID(int id)
        {
            return _db.Students.FirstOrDefault(a=>a.ID == id);
        }
        public void DeleteStudent(int id)
        {
            var stdPermission = _db.Permissions.Where(a => a.StdId==id).ToList();
            foreach (var permission in stdPermission)
            {

            if (permission!=null)
            {
                _db.Permissions.Remove(permission);
               
            }
            }
            var stdAttendance = _db.Attendances.Where(a => a.User.ID == id).ToList();
            foreach(var attendance in stdAttendance)
            {
                if (attendance!=null)
                {

                    _db.Attendances.Remove(attendance);
                }
            }
            var std = _db.Students.FirstOrDefault(a => a.ID ==id);
            _db.Students.Remove(std);
            _db.SaveChanges();
        }

        public void Add(Student std)
        {
            _db.Students.Add(std);
            _db.SaveChanges();
        }
        public void UpdateStd(Student std)
        {
            var existingStudent = _db.Students.FirstOrDefault(a => a.ID == std.ID);
            if (existingStudent != null)
            {
                _db.Entry(existingStudent).State = EntityState.Detached; // Detach the existing entity
                _db.Entry(std).State = EntityState.Modified; // Attach and mark as modified
                _db.SaveChanges();
            }
        }
        public void AddStudents(List<Student> students)
        {
            _db.Students.AddRange(students);
            _db.SaveChanges();
        }


    }
}
