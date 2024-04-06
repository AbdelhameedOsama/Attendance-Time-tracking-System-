using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;

namespace Attendance_Time_Tracking.Models
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late
    }
    public class Attendance
    {
        public int UserId { get; set; }
        public DateOnly Date { get; set; }

        [DefaultValue(1)]
        public AttendanceStatus Status { get; set; }

        public TimeOnly? Arrival_Time { get; set; }

        public TimeOnly? Departure_Time { get; set; }

        public virtual User User { get; set; }
    }

}