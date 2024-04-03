using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Time_Tracking.Models
{

    [Table("Intakes")]
    public class Intake
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name="IntakeName")]
        public string Name { get; set; }

        [Required]

        public int ProgramID { get; set; } 
        public Programs Program { get; set; }


        public ICollection<Track> Tracks { get; set; }

    }
}
