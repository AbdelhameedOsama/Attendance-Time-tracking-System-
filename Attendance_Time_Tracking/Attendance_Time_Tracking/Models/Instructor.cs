using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public class Instructor: User
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        public virtual User User { get; set; }

    }
}
