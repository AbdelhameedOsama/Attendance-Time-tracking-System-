using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Repos
{
	public interface IUserRepo
	{
		public List<User> GetAll();
        User GetUser(string Name, string Password);
		int GetUserId(ClaimsPrincipal user);
        public void DeleteUser(int id);
        public void UpdateUser(User user);
        public void AddUser(User user);


    }
	public class UserRepo : IUserRepo
	{
		readonly AttendanceContext db;
		public UserRepo(AttendanceContext db)
		{
			this.db = db;
		}
        public List<User> GetAll()
        {
            return db.Users.ToList();
        }
        public User GetUser(string Email, string Password)
		{
			return db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
		}

		public int GetUserId(ClaimsPrincipal user)
		{
			var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

			return Convert.ToInt32(userIdClaim.Value);
		}

        public void DeleteUser(int id)
        {
            // if this instructor is supervisor so you must put another instructor instead of him
           /* var instTrack = db.Tracks.FirstOrDefault(a => a.SupID==id);
            if (instTrack != null)
            {
                instTrack.SupID=NewSupervisorId??0;
                db.Tracks.Update(instTrack);
            }
            var InstPermission = db.Permissions.FirstOrDefault(a => a.SupId==id);
            if (InstPermission!=null)
            {
                InstPermission.SupId=NewSupervisorId;
                db.Permissions.Update(InstPermission);

            }*/
            var InstAttendance = db.Attendances.Where(a => a.User.ID == id).ToList();
            foreach (var attendance in InstAttendance)
            {
                if (attendance!=null)
                {
                    db.Attendances.Remove(attendance);
                }
            }
            
            var inst = db.Users.FirstOrDefault(a => a.ID ==id);
            db.Users.Remove(inst);
            db.SaveChanges();
        }

        public void AddUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            var existingUser = db.Users.FirstOrDefault(a => a.ID == user.ID);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Address= user.Address;
                existingUser.Phone= user.Phone;
                existingUser.Name= user.Name;
               /* db.Entry(existingUser).State = EntityState.Detached;
                db.Entry(user).State = EntityState.Modified; */

                db.SaveChanges();
            }
        }

    }
}
