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
        Task<Track> GetTrackBySupervisorId(int id);
        Task<Schedule> CreateSchedule(Schedule schedule);
        Task<List<Schedule>> GetSchedulesBySupId(int id);
        Task<List<Schedule>> GetSchedules();
        Task<List<Track>> TracksInScheduels();
        Task<List<Attendance>> GetAttendancesByInstructorID(int id);
        public void ChangeInstructorToSupervisor(int id);

        public List<Instructor> GetAll();
        public void AddInst(Instructor instructor);
        public void UpdateInst(Instructor inst);

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
        public async Task<Track> GetTrackBySupervisorId(int id)
        {
            var track= await db.Tracks.Where(t=>t.SupID==id).FirstOrDefaultAsync();
            return track;
        }
        public async Task<Schedule> CreateSchedule(Schedule schedule)
        {
			db.Schedules.Add(schedule);
			await db.SaveChangesAsync();
			return schedule;
		}
        public async Task<List<Schedule>> GetSchedulesBySupId(int id)
        {
            return await db.Schedules.Where(s => s.SupId == id).Include(t => t.Track).ToListAsync();
        }
        public async Task<List<Schedule>> GetSchedules()
        {
            return await db.Schedules.Include(t => t.Track).ToListAsync();
        }
        public async Task<List<Track>> TracksInScheduels()
        {
            // Query to retrieve unique track IDs along with related data
            var uniqueTrackIDs = await db.Schedules
                .Select(schedule => schedule.TrackId)
                .Distinct()
                .ToListAsync();
            List<Track> tracks = new List<Track>();
            foreach(var trackID in uniqueTrackIDs)
            {
                var track = await db.Tracks
                    .Where(t => t.ID == trackID)
                    .FirstOrDefaultAsync();
                tracks.Add(track);
            }

            return tracks;
        }
        public async Task<List<Attendance>> GetAttendancesByInstructorID(int id)
        {
            var attendances = await db.Attendances
                .Where(a => a.User.ID == id)
                .ToListAsync();
            return attendances;
        }
        public void ChangeInstructorToSupervisor(int id)
        {
            var user = db.Users.FirstOrDefault(a => a.ID== id);
            if (user != null)
            {
                user.Role=UserRole.Supervisor;
            }
            db.SaveChanges();
        }

        public List<Instructor> GetAll()
        {
           return db.Instructors.ToList();
        }
        public void AddInst(Instructor instructor)
        {
            db.Instructors.Add(instructor);
            db.SaveChanges();
        }
        public void UpdateInst(Instructor inst)
        {
            var existingUser = db.Users.FirstOrDefault(a => a.ID == inst.ID);
            if (existingUser != null)
            {
                existingUser.Email = inst.Email;
                existingUser.Password = inst.Password;
                existingUser.Address= inst.Address;
                existingUser.Phone= inst.Phone;
                existingUser.Name= inst.Name;
                db.SaveChanges();
            }
        }
    }
}
