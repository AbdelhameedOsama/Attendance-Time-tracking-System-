using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;

namespace Attendance_Time_Tracking.Repos
{
    public interface ITrackRepo
    {
        public List<Track> GetAllTracks();
    }
    public class TrackRepo:ITrackRepo
    {
        readonly AttendanceContext db;
        public TrackRepo(AttendanceContext _db)
        {
            db = _db;
        }
        public List<Track> GetAllTracks()
        {
            return db.Tracks.ToList();
        }
    }

}
