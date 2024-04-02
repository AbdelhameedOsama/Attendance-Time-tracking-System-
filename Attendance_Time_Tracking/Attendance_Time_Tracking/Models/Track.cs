using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Attendance_Time_Tracking.Models
{
    [Table("Tracks")]
    public class Track
    {
        [Key]
		[Display(Name = "ID")]
		public int TrackId { get; set; }

        [Required]
		[Display(Name = "TrackName")]
		public string Name { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Status { get; set; }

        /*        [ForeignKey("ProgramId")]
                public Programs Program { get; set; }
                public int ProgramId { get; set; }*/

        [Required]
        [ForeignKey("IntakeId")]
        public Intake Intake { get; set; }
        public int IntakeId { get; set; }


        [Required]
        public int SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        public Instructor Supervisor { get; set; }

    }

}
