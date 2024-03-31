namespace Attendance_Time_Tracking.Models
{
    public enum Emp_Types { Security, Student_Affairs}
    public class Employee : User
    {
        public Emp_Types Type { get; set; }
    }
}
