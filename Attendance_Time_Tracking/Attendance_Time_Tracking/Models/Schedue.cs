using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    [Table("Schedue")]
    public class Schedue
    {
        [Key]
        public int ScheduleId { get; set; }



        public int TrackId { get; set; }
        [ForeignKey("TrackId")]
        public Track Track { get; set; }


        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }
        public int InstructorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
    }
}
