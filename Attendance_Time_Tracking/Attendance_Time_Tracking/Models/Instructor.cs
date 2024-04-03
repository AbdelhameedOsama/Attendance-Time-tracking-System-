using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public class Instructor: User
    {
        //[ForeignKey("User")]
        //public int ID { get; set; }

        //public virtual User User { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Track> Tracks { get; set; } = new List<Track>();
    }

}
