using Attendance_Time_Tracking.Data;

namespace Attendance_Time_Tracking.Repos
{
    public interface IInstructorRepo
    {
        
    }
    public class InstructorRepo : IInstructorRepo
    {
        readonly AttendanceContext db;
        public InstructorRepo(AttendanceContext _db)
        {
            db = _db;
        }
    }
}
