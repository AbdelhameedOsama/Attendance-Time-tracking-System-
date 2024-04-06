using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace Attendance_Time_Tracking.Repos
{
	public interface IStudentRepo
	{
        public List<Student> GetAll();
        public Student GetStudentByID(int id);
        public void DeleteStudent(int id);
        public void Add(Student std);
        public void UpdateStd(Student std);

    }
	public class StudentRepo : IStudentRepo
	{
		readonly AttendanceContext db;
		public StudentRepo(AttendanceContext _db)
		{
			db = _db;
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
