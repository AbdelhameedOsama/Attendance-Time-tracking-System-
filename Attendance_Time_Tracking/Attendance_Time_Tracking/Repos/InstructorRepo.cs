using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Repos
{
    public interface IInstructorRepo
    {
        Task<User> GetUserByID(int id);  
        Task<List<Permission>> GetPermissionsBySupevisorID(int id);
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
    }
}
