using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public class Student:User
    {
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Specialization { get; set; }
        public virtual User student { get; set; } = null!;
        public DateOnly Graduation_year { get; set; }


        public int TrackId { get; set; }
        // [ForeignKey("TrackId")]
        // public Track TrackNavigation { get; set; }
        public List<Permission> Permissions { get; set; }=new List<Permission>();




    }
}
