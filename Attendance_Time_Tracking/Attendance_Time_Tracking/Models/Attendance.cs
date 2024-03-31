using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }


        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedue Schedule { get; set; }

        public bool IsPresent { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Name { get; set; }


        [ForeignKey("ProgramId")]
        public Programs Program { get; set; }
        public int ProgramId { get; set; }


        [ForeignKey("TrackId")]
        public Track Track { get; set; }
        public int TrackId { get; set; }

    }
}
