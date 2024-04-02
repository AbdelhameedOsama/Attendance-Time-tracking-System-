using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    [Table("Schedue")]
    public class Schedue
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Track")]
        public int TrackId { get; set; }
        public Track Track { get; set; }


        [ForeignKey("Instructor")]
        public int SupId { get; set; }
        public Instructor Instructor { get; set; }

        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public DateTime Date { get; set; }
    }
}
