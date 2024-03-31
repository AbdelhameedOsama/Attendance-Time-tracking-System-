using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Repos
{
	public interface IUserRepo
	{
		User GetUser(string Name, string Password);
		int GetUserId(ClaimsPrincipal user);

	}
	public class UserRepo : IUserRepo
	{
		readonly AttendanceContext db;
		public UserRepo(AttendanceContext db)
		{
			this.db = db;
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
	}
}
