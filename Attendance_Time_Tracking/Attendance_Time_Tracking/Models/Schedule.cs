using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    public class Schedule
    {
        public int ID { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }


        public int SupId { get; set; }
        public Instructor Supervisor { get; set; }

        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public DateTime Date { get; set; }
    }
}
