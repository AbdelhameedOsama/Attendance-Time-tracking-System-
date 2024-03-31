using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{
    [Table("Tracks")]
    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public string Name { get; set; }

        public Boolean Status { get; set; }

        [ForeignKey("ProgramId")]
        public Programs Program { get; set; }
        public int ProgramId { get; set; }

        [ForeignKey("IntakeId")]
        public Intake Intake { get; set; }
        public int IntakeId { get; set; }


        public int SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        public Instructor Supervisor { get; set; }

    }

}
