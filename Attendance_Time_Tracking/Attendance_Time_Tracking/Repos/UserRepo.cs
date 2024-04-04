using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using System.Security.Claims;

namespace Attendance_Time_Tracking.Repos
{
	public interface IUserRepo
	{
		User GetUser(string Name, string Password);
		int GetUserId(ClaimsPrincipal user);
		Task<User> UpdateUser(User user);

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
		public async Task<User> UpdateUser(User user)
		{
			var existingUser = db.Users.FirstOrDefault(u => u.ID == user.ID);
			if (existingUser != null)
			{
				existingUser.Email = user.Email;
				existingUser.Password = user.Password;
				existingUser.Name = user.Name;
				existingUser.Phone = user.Phone;
				
				await db.SaveChangesAsync();
			}
			return user;
		}
	}
}
