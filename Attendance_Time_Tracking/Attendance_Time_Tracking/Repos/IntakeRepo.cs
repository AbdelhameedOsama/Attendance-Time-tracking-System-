using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;

namespace Attendance_Time_Tracking.Repos
{
    public interface IIntakeRepo
    {
        public List<Intake> GetIntakeList();
    }
    public class IntakeRepo:IIntakeRepo
    {
        readonly AttendanceContext db;
        public IntakeRepo(AttendanceContext _db)
        {
            db = _db;
        }
        public List<Intake> GetIntakeList()
        {
            return db.Intakes.ToList();
        }
    }
}
