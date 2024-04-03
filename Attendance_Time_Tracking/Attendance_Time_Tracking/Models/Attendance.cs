using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;

namespace Attendance_Time_Tracking.Models
{
    public class Attendance
    {

        public int UserId { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue("Absent")]
        public string Status { get; set; }

        public DateTime Arrival_Time { get; set; }
 
        public DateTime? Departure_Time { get; set; }

        public virtual User User { get; set; }
    }

}