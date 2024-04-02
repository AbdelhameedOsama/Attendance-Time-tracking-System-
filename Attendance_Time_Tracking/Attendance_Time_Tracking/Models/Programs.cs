using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Time_Tracking.Models
{
    public enum ProgramType
    {
        IntensiveTraining,
        SummerProgram,
        ProfessionalProgram
    }
    [Table("Programs")]
    public class Programs
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public ProgramType Name { get; set; }

        public virtual List<Intake> ProgramIntakes { get; set; } = new List<Intake>();
    }
}
