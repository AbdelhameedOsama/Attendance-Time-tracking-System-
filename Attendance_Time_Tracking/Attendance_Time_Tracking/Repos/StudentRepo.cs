using Attendance_Time_Tracking.Data;

namespace Attendance_Time_Tracking.Repos
{
	public interface IStudentRepo
	{
	}
	public class StudentRepo : IStudentRepo
	{
		readonly AttendanceContext db;
		public StudentRepo(AttendanceContext _db)
		{
			db = _db;
		}

	}
}
