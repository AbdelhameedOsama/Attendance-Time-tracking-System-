namespace Attendance_Time_Tracking.Models
{
    public class Instructor: User
    {
        public bool IsSupervisor { get; set; }
		public virtual User InstructorNavigation { get; set; } = null!;
		public List<Attendance> Attendances { get; set; }=new List<Attendance>();
	}
}
