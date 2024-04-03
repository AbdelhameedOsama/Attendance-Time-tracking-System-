using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Repos
{
    public interface IInstructorRepo
    {
        Task<User> GetUserByID(int id);  
        Task<List<Permission>> GetPermissionsBySupevisorID(int id);
        Task<Permission> DeletePermission(Permission p);
        Task<Permission> GetPermission(int id,DateTime date);
        Task<Permission> ApprovePermission(Permission p);
        Task<Permission> RejectPermission(Permission p);
    }
    public class InstructorRepo : IInstructorRepo
    {
        readonly AttendanceContext db;
        public InstructorRepo(AttendanceContext _db)
        {
            db = _db;
        }
        public async Task<User> GetUserByID(int id)
        {
            return await db.Users.FindAsync(id);
		}
        public async Task<List<Permission>> GetPermissionsBySupevisorID(int id)
        {
            return await db.Permissions.Where(p => p.SupId == id).Include(s=>s.Student).ToListAsync();
		}
        public async Task<Permission> DeletePermission(Permission p)
        {
            db.Permissions.Remove(p);
			await db.SaveChangesAsync();
			return p;
        }
        public async Task<Permission> ApprovePermission(Permission p)
        {
            p.Status = PermissionStatus.Approved;
            db.Permissions.Update(p);
            await db.SaveChangesAsync();
            return p;
        }
        public async Task<Permission> RejectPermission(Permission p)
        {
			p.Status = PermissionStatus.Rejected;
			db.Permissions.Update(p);
			await db.SaveChangesAsync();
			return p;
		}
        public async Task <Permission> GetPermission(int id,DateTime date)
        {
			List<Permission> permissions = await db.Permissions.Where(p => p.StdId == id).ToListAsync();
            foreach (var permission in permissions)
            {
				if(permission.Date.ToString("g") == date.ToString("g"))
                {
					return permission;
				}
			}
            return null;

		}
    }
}
