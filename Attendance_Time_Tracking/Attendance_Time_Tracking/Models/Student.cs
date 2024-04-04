using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public class Student:User
    {
        //[ForeignKey("User")]
       // public int ID { get; set; }

        public string University { get; set; }
        public string Faculty { get; set; }
        public string Specialization { get; set; }
        //public virtual User student { get; set; } = null!;
        public DateOnly Graduation_year { get; set; }

        [ForeignKey("Track")]
        public int TrackId { get; set; }
        public Track Track { get; set; }

        public List<Permission> Permissions { get; set; }=new List<Permission>();

        //public int AttendanceDegrees { get; set; } = 250;



    }
}
