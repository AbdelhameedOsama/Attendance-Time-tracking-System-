using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Attendance_Time_Tracking.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Date")]
        public DateTime Date { get; set; }

        public string Status { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }

        public virtual User User { get; set; }
    }

}