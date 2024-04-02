using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    public enum Emp_Types { Security, Student_Affairs}
    public class Employee : User
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        public Emp_Types Type { get; set; }

        public virtual User User { get; set; }
    }
}
