using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;

namespace Attendance_Time_Tracking.Repos
{
    public interface IAdminRepo
    {
        public List<Track> GetAllTracks();
    }
    public class AdminRepo: IAdminRepo
    {

        readonly AttendanceContext db;
        public AdminRepo(AttendanceContext _db)
        {
            db = _db;
        }
      public List<Track> GetAllTracks()
        {
            return db.Tracks.ToList() ;
        }
    }
}
