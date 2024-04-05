using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Attendance_Time_Tracking.Models
{
    public class Schedule
    {
        public int ID { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }

        [AllowNull]
        public int? SupId { get; set; }
        public Instructor Supervisor { get; set; }

        public TimeSpan Start_Time { get; set; }
        public TimeSpan End_Time { get; set; }
        public DateOnly Date { get; set; }
    }
}
