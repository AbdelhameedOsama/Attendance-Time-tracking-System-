using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Attendance_Time_Tracking.Models
{
    [Table("Tracks")]
    public class Track
    {
        [Key]
		public int ID { get; set; }

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
        public int IntakeId { get; set; }
        public Intake Intake { get; set; }


        [ForeignKey("Supervisor")]
        public int SupID { get; set; }
        public virtual Instructor Supervisor { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public virtual ICollection<Student> Instructors { get; set; } = new List<Student>();

    }

}
